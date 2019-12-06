namespace Project.Http
{
    using System;

    using Microsoft.CorrelationVector;

    public sealed class CorrelationVectorFeature : ICorrelationVectorFeature
    {
        public CorrelationVectorFeature()
        {
            CV = new CorrelationVector(Guid.NewGuid());
        }

        public CorrelationVectorFeature(string correlationVector)
        {
            CV = CorrelationVector.Parse(correlationVector);
        }

        public CorrelationVector CV { get; }
    }
}