using System;
namespace AB.Ecommerce.Gros.Business
{
	public class BusinessException : Exception
	{
		  
            public string Code { get; }
            public string[]? Args { get; }
            public System.Net.HttpStatusCode Status { get; }


            public BusinessException(string code, params string[] args)
                : this(code, System.Net.HttpStatusCode.Forbidden, args)
            {
            }

            public BusinessException(string code, System.Net.HttpStatusCode status, params string[] args)
                : base(FormatMessage(code, args))
            { 
                Code = code;
                Args = args;
                Status = status;
            }

        static string FormatMessage(string code, params string[] args)
        {
            var message = BusinessErrors.L[code];
            if (args!=null)
            { 
                for (var i = 0; i < args.Length; i++)
                {
                    message = message.Replace($"{{{i}}}", args[i]);
                }
            }
            return message;
        }

    }
}

