namespace Project.Http
{
    using Microsoft.CorrelationVector;

    public interface ICorrelationVectorFeature
    {
        CorrelationVector CV { get; }
    }
}