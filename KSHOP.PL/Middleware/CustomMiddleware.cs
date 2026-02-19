namespace KSHOP.PL.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("processing prequest");
            await _next(context);
            Console.WriteLine("processing response");
        }
    }
}
