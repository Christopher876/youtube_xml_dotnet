using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

class VideoEqualityChecker : IEqualityComparer<Video>
{
    public bool Equals([AllowNull] Video x, [AllowNull] Video y)
    {
        return (x.id == y.id) ? true:false;
    }

    public int GetHashCode([DisallowNull] Video obj)
    {
        return obj.GetHashCode();
    }
}