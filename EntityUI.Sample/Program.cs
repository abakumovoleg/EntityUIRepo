using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;

namespace EntityUI.Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var container = new UnityContainer();
            var storage = CreateStorage();
            container.RegisterInstance<IEntityProvider>(storage);

            var engine = new Engine(storage, new UnityDependencyContainer(container));
            
            var form = engine.CreateReestrForm<UserDto>();

            Application.Run(form);
            
        }

        private static InMemoryEntityProvider CreateStorage()
        {
            var storage = new InMemoryEntityProvider();

            var moscow = new FilialDto
            {
                Id = 1,
                Name = "Moscow"
            };

            var cyprus = new FilialDto
            {
                Id = 2,
                Name = "Cyprus"
            };

            var uk = new FilialDto
            {
                Id = 3,
                Name = "UK"
            };

            var admin = new RoleDto
            {
                Id = 1,
                Name = "Admin",
                Filial = new List<FilialDto> {moscow, uk}
            };

            var simpleUser = new RoleDto
            {
                Id = 2,
                Name = "User",
                Filial = new List<FilialDto> {cyprus, uk}
            };

            var roles = new List<RoleDto>
            {
                admin,
                simpleUser
            };



            var filials = new List<FilialDto>
            {
                moscow,
                cyprus,
                uk
            };

            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Email = "vasya@pupkin.com",
                    Name = "Vasya Pupkin",
                    Role = new List<RoleDto>{ simpleUser},
                    Age = 15,
                    Filial = moscow
                },
                new UserDto
                {
                    Id = 2,
                    Email = "abakumovoleg@gmail.com",
                    Name = "Oleg",
                    Role = new List<RoleDto> { admin, simpleUser},
                    Age = 29,
                    Filial = cyprus
                }
            };

            foreach (var userDto in users)
                storage.Add(userDto);

            foreach (var roleDto in roles)
                storage.Add(roleDto);

            foreach (var filialDto in filials)
                storage.Add(filialDto);

            return storage;
        }
    }
}
