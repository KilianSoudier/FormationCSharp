namespace Formation.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;
                                                    //Permet d'éviter de faire un try catch par fonction du controller
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)  //Ici le logger est générique La classe dans laquelle l'exception arrivera changera le nom automatiquement.
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status502BadGateway, title: "OSKOUUUUUR") //Renvoie une exception rfc : objet standard qui récupère plusieurs infos sur l'exception
                .ExecuteAsync(context);
            }
        }
    }
}
