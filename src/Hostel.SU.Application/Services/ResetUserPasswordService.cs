using Hostel.Shared.Kernel;
using Hostel.SU.Domain;

namespace Hostel.SU.Application
{
    /// <inheritdoc/>
    internal class ResetUserPasswordService : IResetUserPasswordService
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _userRepository;

        /// <inheritdoc cref="IResetPasswordTokenRepository"/>
        private readonly IResetPasswordTokenRepository _tokenRepository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IPasswordService"/>
        private readonly IPasswordService _passwordService;

        public ResetUserPasswordService(IUserRepository userRepository,
            IResetPasswordTokenRepository tokenRepository,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        #endregion

        /// <inheritdoc/>
        public async Task ChangePasswordAsync(Guid token, string newPassword, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var tokenModel = await _tokenRepository.GetByIdAsync(token, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityResetPasswordToken,
                        DomainFieldCodes.DomainFieldId, token);

                var user = await _userRepository.GetByIdAsync(tokenModel.UserId)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        DomainFieldCodes.DomainFieldId, tokenModel.UserId);

                user.ChangePassword(_passwordService.GetHashPassword(newPassword));
                user.ChangeStatus(UserStatuses.Active);

                tokenModel.MarkAsUsed();

                _userRepository.Update(user);
                _tokenRepository.Update(tokenModel);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task CreateResetPasswordAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        DomainFieldCodes.DomainFieldId,
                        userId);

                var token = UserResetPasswordToken.Create(user.Id);

                user.ChangeStatus(UserStatuses.Inactive);

                _userRepository.Update(user);
                _tokenRepository.Add(token);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task SetExpiredTokenAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var tokens = await _tokenRepository.GetAllAsync(cancellationToken);

                tokens.Where(x => x.ExpiredAt > DateTime.UtcNow
                    && (x.Status != ResetPasswordStatuses.Expired || x.Status != ResetPasswordStatuses.Used)).ToList();

                foreach (var token in tokens)
                {
                    token.MarkAsExpired();
                    _tokenRepository.Update(token);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
