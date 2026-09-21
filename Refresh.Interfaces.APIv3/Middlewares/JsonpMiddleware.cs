using Bunkum.Listener.Request;
using Bunkum.Core;
using Bunkum.Core.Database;
using Bunkum.Core.Endpoints.Middlewares;
using Refresh.Interfaces.APIv3;
using System.Text;

namespace Refresh.APIv3.Middlewares;

public class PspJsonp : IMiddleware
{
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        next();
        string? format = context.Query["format"];
        if (format == "psp")
        {
            string callbackname = context.Query["callback"] ?? "result";
            context.ResponseType = "application/javascript; charset=utf-8";

            byte[] origjsonbytes = context.ResponseStream.ToArray();
            string jsonString = Encoding.UTF8.GetString(origjsonbytes);

            string jsonp = $"{callbackname}({jsonString});";
            byte[] jsonpbyte = Encoding.UTF8.GetBytes(jsonp);
            context.ResponseStream.SetLength(0);
            context.ResponseStream.Position = 0;
            context.ResponseStream.Write(jsonpbyte,0, jsonpbyte.Length);
        }
        
    }
}