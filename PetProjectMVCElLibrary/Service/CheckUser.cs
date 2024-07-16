using System.Security.Claims;

namespace PetProjectMVCElLibrary.Service
{
    public static class CheckUser
    {
        /// <summary>
        ///     Check user is active
        /// </summary>
        /// <param name="context"></param>
        /// <param name="idUser">out UserId</param>
        /// <returns></returns>
        public static bool IsUserTry(IHttpContextAccessor context, out Guid idUser)
        {
            if (context is null) throw new ArgumentNullException(@"Context is null");
            idUser = Guid.Empty;
            ClaimsPrincipal? currentUser = context.HttpContext?.User;
            Claim? user = currentUser?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
            if (user != null)
            {
                if (Guid.TryParse(user.Value, out idUser))
                    return true;
                return false;
            }
            return false;
        }
    }
}
