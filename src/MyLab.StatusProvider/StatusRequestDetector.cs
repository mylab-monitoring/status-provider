using Microsoft.AspNetCore.Http;

namespace MyLab.StatusProvider
{
    class StatusRequestDetector
    {
        private readonly string _normPath;

        public StatusRequestDetector(string path)
        {
            _normPath = '/' + path.Trim('/');
        }

        public string DetectAndGetRelatedPath(HttpRequest request)
        {
            return DetectAndGetRelatedPath(request.Method, request.Path);
        }

        public string DetectAndGetRelatedPath(string method, PathString path)
        {

            if (method != "GET" ||
                (path != ("/" + _normPath) && !path.StartsWithSegments(_normPath)))
                return null;

            return path.Value.Substring(path.Value.IndexOf(_normPath) + _normPath.Length);
        }
    }
}