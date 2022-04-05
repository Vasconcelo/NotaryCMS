using Notary.CMS.DataAccess.Interfaces;
using NUnit.Framework;
using System;

namespace CMS.Tests
{
    
    public class Tests
    {
        private  IApplicationRepository _applicationRepository;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Read line");
            Assert.Pass();            
        }
    }
}