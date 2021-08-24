using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelVillaClient.Helpers
{
    public static class JRunExt
    {

        public static async ValueTask ToastrSuccess(this IJSRuntime JSRuntime, string message)
        {
            //await JSRuntime.InvokeVoidAsync("showmsg", "success", message);
            await JSRuntime.InvokeVoidAsync("ShowToastr", "success", message);

        }
        public static async ValueTask ToastrError(this IJSRuntime JSRuntime, string message)
        {
            //await JSRuntime.InvokeVoidAsync("showmsg", "error", message);
            await JSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

    }
}
