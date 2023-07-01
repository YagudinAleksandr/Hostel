using Blazored.Toast;
using Microsoft.AspNetCore.Components;

namespace Hostel.BlazorUI.Components
{
    public partial class ToastNotifyComponent
    {
        [CascadingParameter]
        private BlazoredToast ToastInstance { get; set; } = default!;

        [Parameter]
        public string? Title { get; set; }
        [Parameter]
        public string? ToastParam { get; set; }
        [Parameter]
        public string Type { get; set; }
    }
}
