using System;
using FluentAssertions;
using KS.Fiks.IO.Client.Amqp;
using KS.Fiks.IO.Client.Exceptions;
using KS.Fiks.IO.Client.Models;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace KS.Fiks.IO.Client.Tests.Amqp
{
    public class AmqpHandlerTests
    {
        private AmqpHandlerFixture _fixture;

        public AmqpHandlerTests()
        {
            _fixture = new AmqpHandlerFixture();
        }

        [Fact]
        public void CreatesModelWhenConstructed()
        {
            var sut = _fixture.CreateSut();

            _fixture.ConnectionFactoryMock.Verify(_ => _.CreateConnection(), Times.Once);
            _fixture.ConnectionMock.Verify(_ => _.CreateModel(), Times.Once);
        }

        [Fact]
        public void ThrowsExceptionWhenConnectionFactoryThrows()
        {
            Assert.Throws<AmqpConnectionFailedException>(() =>
                _fixture.WhereConnectionfactoryThrowsException().CreateSut());
        }

        [Fact]
        public void ThrowsExceptionWhenConnectionThrows()
        {
            Assert.Throws<AmqpConnectionFailedException>(() =>
                _fixture.WhereConnectionThrowsException().CreateSut());
        }

        [Fact]
        public void AddReceivedListenerCreatesNewConsumer()
        {
            var sut = _fixture.CreateSut();

            var handler = new EventHandler<MessageReceivedArgs>((a, _) => { });

            sut.AddReceivedListener(handler);

            _fixture.AmqpConsumerFactoryMock.Verify(_ => _.CreateReceiveConsumer(It.IsAny<IModel>()));
        }

        [Fact]
        public void AddReceivedListenerAddsHandlerToReceivedEvent()
        {
            var sut = _fixture.CreateSut();

            var counter = 0;
            var handler = new EventHandler<MessageReceivedArgs>((a, _) => { counter++; });

            sut.AddReceivedListener(handler);

            _fixture.AmqpReceiveConsumerMock.Raise(_ => _.Received += null, this, null);
            counter.Should().Be(1);
        }
    }
}