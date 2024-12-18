using EmotionsApp.Controllers;
using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmotionsApp.UnitTests
{
    [TestClass]
    public class PsychologistControllerTests
    {
        private Mock<IPsychologistService> _serviceMock;
        private Mock<ILogger<PsychologistController>> _loggerMock;
        private PsychologistController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceMock = new Mock<IPsychologistService>();
            _loggerMock = new Mock<ILogger<PsychologistController>>();
            _controller = new PsychologistController(_serviceMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetByUser_Success()
        {
            // Подготавливаем данные для теста 
            var userId = Guid.NewGuid();
            var psychologists = new List<Psychologist>
            {
                new Psychologist { Id = Guid.NewGuid(), Name = "Психолог 1", ContactInfo = "Контакт 1" },
                new Psychologist { Id = Guid.NewGuid(), Name = "Психолог 2", ContactInfo = "Контакт 2" }
            };

            _serviceMock.Setup(x => x.GetPsychologistsByUserAsync(userId)).ReturnsAsync(psychologists);

            // Выполняем действие 
            var result = await _controller.GetByUser(userId);

            // Проверяем результат 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult)); // Убедимся, что возвращен OkObjectResult 
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(psychologists, okResult.Value); // Проверяем, что возвращены корректные данные 
        }

        [TestMethod]
        public async Task GetByUser_NotFound()
        {
            // Подготавливаем данные для теста 
            var userId = Guid.NewGuid();
            var psychologists = new List<Psychologist>(); // Пустой список 

            _serviceMock.Setup(x => x.GetPsychologistsByUserAsync(userId)).ReturnsAsync(psychologists);

            // Выполняем действие 
            var result = await _controller.GetByUser(userId);

            // Проверяем результат 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult)); // Ожидаем OkObjectResult 
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(0, ((List<Psychologist>)okResult.Value).Count); // Проверяем, что список пуст 
        }

        [TestMethod]
        public async Task GetByUser_InternalServerError()
        {
            // Подготавливаем данные для теста 
            var userId = Guid.NewGuid();

            _serviceMock.Setup(x => x.GetPsychologistsByUserAsync(userId))
                        .ThrowsAsync(new Exception("Внутренняя ошибка сервера")); // Исключение 

            // Выполняем действие 
            var result = await _controller.GetByUser(userId);

            // Проверяем результат 
            Assert.IsInstanceOfType(result, typeof(ObjectResult)); // Убедимся, что возвращен ObjectResult 
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode); // Проверяем код ошибки 500 
            Assert.AreEqual("Внутренняя ошибка сервера", objectResult.Value); // Проверяем сообщение об ошибке 
        }
    }
}