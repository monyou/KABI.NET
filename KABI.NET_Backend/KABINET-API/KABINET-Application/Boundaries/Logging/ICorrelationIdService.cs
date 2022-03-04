using System;

namespace KABINET_Application.Boundaries.Logging
{
    public interface ICorrelationIdService
    {
        Guid Get();
    }
}
