using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class SecurityDbContextData 
    {

        public static async Task SeedUserAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager) {

            if (!userManager.Users.Any()) {
                var usuario = new Usuario
                {
                    Nombre = "Sam",
                    Apellido = "Zele",
                    UserName = "Sizm",
                    Email = "sam.zele@gmail.com",
                    Direccion = new Direccion
                    {
                        Calle = "Colonia San Sebastian",
                        Ciudad = "Diriamba",
                        CodigoPostal = "46300",
                        Departamento = "Carazo",
                        Pais="Nicaragua"
                    }
                };

                await userManager.CreateAsync(usuario, "SamZele2025$");
            }


            if (!roleManager.Roles.Any()) {
                var role = new IdentityRole
                {
                    Name = "ADMIN"
                };
                await roleManager.CreateAsync(role);
            }


        }

    }
}
