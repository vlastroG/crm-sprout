using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Consulting.Models {
    public class ClaimsComparer : IEqualityComparer<Claim> {
        public bool Equals(Claim? x, Claim? y) {
            if(ReferenceEquals(x, null) || ReferenceEquals(y, null)) {
                return false;
            }
            if(ReferenceEquals(x, y)) {
                return true;
            }
            return x.Type == y.Type
                && x.Value == y.Value;
        }

        public int GetHashCode([DisallowNull] Claim obj) {
            return obj.Type.GetHashCode() + obj.Value.GetHashCode();
        }
    }
}
