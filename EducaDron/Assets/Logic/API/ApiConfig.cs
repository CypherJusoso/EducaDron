using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.API
{
    public static class ApiConfig
    {
        public static string BaseUrl =
            "https://educadron-api-jfk-bkfcf9ckdqbjfngd.francecentral-01.azurewebsites.net";

        public static string Build(string relativePath)
        {
            var b = BaseUrl.TrimEnd('/');
            var r = relativePath.StartsWith("/") ? relativePath : "/" + relativePath;
            return b + r;
        }

        public static string BuildWithQuery(string path, params (string key, string value)[] qs)
        {
            var url = Build(path); // reutilizamos Build para base + path

            if (qs == null || qs.Length == 0) return url;

            var sb = new System.Text.StringBuilder(url);
            sb.Append('?');

            for (int i = 0; i < qs.Length; i++)
            {
                if (i > 0) sb.Append('&');
                sb.Append(Uri.EscapeDataString(qs[i].key))
                  .Append('=')
                  .Append(Uri.EscapeDataString(qs[i].value ?? ""));
            }

            return sb.ToString();
        }
    }
}
