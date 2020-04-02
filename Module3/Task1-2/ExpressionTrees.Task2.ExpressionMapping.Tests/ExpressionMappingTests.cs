using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void Map_FooAndBar_ReturnTheSameNameAndId()
        {
            var sourceFoo = new Foo {Id = 100, Name = "Food", ShortName = "ShortName"};
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.GenerateDefaultMapping<Foo, Bar>();

            var destinationBar = mapper.Map(sourceFoo);

            Assert.IsNotNull(destinationBar);

            Assert.AreEqual(destinationBar.Id, sourceFoo.Id);
            Assert.AreEqual(destinationBar.Name, sourceFoo.Name);
            Assert.AreEqual(destinationBar.ShortName, sourceFoo.ShortName);
            Assert.IsNull(destinationBar.FullName);
        }


        [TestMethod]
        public void Map_FooShortNameAndBarName_And_FooNameAndBarFullName_ReturnTheSameValues()
        {
            var sourceFoo = new Foo { Id = 100, Name = "Food", ShortName = "ShortName" };
            var mapGenerator = new MappingGenerator();
            var mappingConfig = mapGenerator
                .GetMappingConfig<Foo, Bar>()
                .ForMember(src => src.ShortName, (dst) => dst.Name)
                .ForMember(src => src.Name, (dst) => dst.FullName);

            var mapper = mapGenerator.GenerateCustomMapping(mappingConfig);

            var destinationBar = mapper.Map(sourceFoo);

            Assert.IsNotNull(destinationBar);

            Assert.AreNotEqual(destinationBar.Id, sourceFoo.Id);
            Assert.AreEqual(destinationBar.Name, sourceFoo.ShortName);
            Assert.AreEqual(destinationBar.FullName, sourceFoo.Name);
            Assert.IsNull(destinationBar.ShortName);
        }
    }
}
