using CorrelationId.Abstractions;
using KABINET_Application.Boundaries.Logging;
using System;

namespace KABINET_Persistence.Services.Logging
{
    public class CorrelationIdService : ICorrelationIdService
    {
        private readonly ICorrelationContextAccessor correlationContextAccessor;

        public CorrelationIdService(ICorrelationContextAccessor correlationContextAccessor) => this.correlationContextAccessor = correlationContextAccessor;

        public Guid Get() => new Guid(this.correlationContextAccessor.CorrelationContext.CorrelationId);
    }
}
