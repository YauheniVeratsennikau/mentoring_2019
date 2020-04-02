/*
 * Create a Mapper to map one class to another
 * 1 - Default by name and type
 * 2 - you can choose fields for mapping
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using ExpressionTrees.Task2.ExpressionMapping;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    using System.Linq.Expressions;


    class User
    {
        public string FullName { get; set; }
        public string Name { get; set; }

        public string PersonalInfo { get; set; }

    }

    class UserVM
    {
        public string Annotation { get; set; }

        public string FullName { get; set; }
        public string Name { get; set; }
    }

    static partial class Program
    {
        static void Run_Mapping(string[] args)
        {
            User testUser = new User() { Name = "Paul", FullName = "Paul Swiss", PersonalInfo = "Some personal info"};

            var generator = new MappingGenerator();

            var defaultMapper = generator.GenerateDefaultMapping<User, UserVM>();
            var defaultUserVm = defaultMapper.Map(testUser);

            Console.WriteLine("Default mapping by name and type of field");
            Console.WriteLine("      User ------- UserVm");
            Console.WriteLine($"Name: {testUser.Name} ------- {defaultUserVm.Name}");
            Console.WriteLine($"FullName: {testUser.FullName} ------- {defaultUserVm.FullName}");
            Console.WriteLine($"PersonalInfo: {testUser.PersonalInfo} ------- Annotation: {defaultUserVm.Annotation ?? "null"}");

            var config = generator
                .GetMappingConfig<User, UserVM>()
                .ForMember(user => user.Name, userVm => userVm.Name)
                .ForMember(user => user.PersonalInfo, userVm => userVm.Annotation);

            var customMapper = generator.GenerateCustomMapping(config);

            var customUserVm = customMapper.Map(testUser);

            Console.WriteLine(" ");
            Console.WriteLine("Custom mapping ");
            Console.WriteLine("      User ------- UserVm");
            Console.WriteLine($"Name: {testUser.Name} ------- {customUserVm.Name}");
            Console.WriteLine($"FullName: {testUser.FullName} ------- {customUserVm.FullName ?? "null"}");
            Console.WriteLine($"PersonalInfo: {testUser.PersonalInfo} ------- Annotation: {customUserVm.Annotation}");

            Console.ReadLine();
        }

    }
}
