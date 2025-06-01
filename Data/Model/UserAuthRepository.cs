using UnityEngine;
using Zenject;

namespace Data
{

    public interface IUserAuthRepository {
        string GetUID();
        string GetToken();
        
    }

    public class UserAuthRepositoryFirebase : IUserAuthRepository {

        private UserAuthModel _model;
        public UserAuthRepositoryFirebase() {
            _model = new UserAuthModel();
        }

        public string GetToken() {

            return _model.token;
        }

        public string GetUID() {
            return _model.uid;
        }
    }
}
