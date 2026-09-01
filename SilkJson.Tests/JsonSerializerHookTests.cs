using System;
using System.Collections.Generic;
using Xunit;

namespace SilkJson.Tests
{
    public class JsonSerializerHookTests : IDisposable
    {
        public JsonSerializerHookTests()
        {
            JsonSerializer.OnSerialize = null;
            JsonSerializer.OnDeserialize = null;
        }

        public void Dispose()
        {
            JsonSerializer.OnSerialize = null;
            JsonSerializer.OnDeserialize = null;
        }

        [Fact]
        public void OnSerialize_WhenHandled_ReplacesDefaultSerialization()
        {
            JsonSerializer.OnSerialize = delegate(object target, out Json json)
            {
                Money money = target as Money;
                json = money == null ? null : new JsonValue(money.Amount + " " + money.Currency);
                return money != null;
            };

            Json result = Json.From(new Money { Amount = 25, Currency = "USD" });

            Assert.Equal("25 USD", result.ToString());
        }

        [Fact]
        public void OnSerialize_WhenNotHandled_UsesDefaultSerialization()
        {
            bool invoked = false;
            JsonSerializer.OnSerialize = delegate(object target, out Json json)
            {
                invoked = true;
                json = null;
                return false;
            };

            Json result = Json.From(42);

            Assert.True(invoked);
            Assert.Equal("42", result.ToString());
        }

        [Fact]
        public void OnSerialize_WhenHandledWithNullJson_ReturnsJsonNull()
        {
            JsonSerializer.OnSerialize = delegate(object target, out Json json)
            {
                json = null;
                return target is Money;
            };

            Json result = Json.From(new Money());

            Assert.True(result.IsMissed);
            Assert.Null(result.ToString());
        }

        [Fact]
        public void OnDeserialize_WhenHandled_ReplacesDefaultDeserialization()
        {
            JsonSerializer.OnDeserialize = DeserializeMoney;

            Money result = Json.Parse("\"19 EUR\"").To<Money>();

            Assert.Equal(19, result.Amount);
            Assert.Equal("EUR", result.Currency);
        }

        [Fact]
        public void OnDeserialize_IsUsedForNestedValues()
        {
            JsonSerializer.OnDeserialize = DeserializeMoney;

            Wallet result = Json.Parse("{\"Balance\":\"7 GBP\"}").To<Wallet>();

            Assert.Equal(7, result.Balance.Amount);
            Assert.Equal("GBP", result.Balance.Currency);
        }

        [Fact]
        public void OnDeserialize_WhenNotHandled_UsesDefaultDeserialization()
        {
            bool invoked = false;
            JsonSerializer.OnDeserialize = delegate(Json json, Type type, out object value)
            {
                invoked = true;
                value = null;
                return false;
            };

            int result = Json.Parse("42").To<int>();

            Assert.True(invoked);
            Assert.Equal(42, result);
        }

        private static bool DeserializeMoney(Json json, Type type, out object value)
        {
            if (type != typeof(Money))
            {
                value = null;
                return false;
            }

            string[] parts = json.String().Split(' ');
            value = new Money
            {
                Amount = int.Parse(parts[0]),
                Currency = parts[1]
            };
            return true;
        }

        private class Money
        {
            public int Amount { get; set; }
            public string Currency { get; set; }
        }

        private class Wallet
        {
            public Money Balance { get; set; }
        }
    }
}
