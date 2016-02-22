angular.module('app').factory('AuthenticationService', function ($http, $q, LocalStorageModule, apiUrl) {

    var state = {
        authorized: false
    };

    function initialize() {
        // this will be called as soon as the application loads (when the user first opens the page)
        var token = LocalStorageModule.get('token');

        if (token) {
            state.authorized = true;
        }
    }
    
        // The only thing this register method takes in is a registration.
        // This registration will be a JavaScript object that contains Username, Email Address, Password, and Confirm Password
        // This is what we'll send over to our api, the method we wrote earlier today
    
    function register(registration) {
        // this will call /api/accounts/register
        return $http.post(apiUrl + 'accounts/register', registration).then(
            function () {
                return response;
            }
        );
    }
    
    // loginData is a JavaScript object that contains the Username and Password from the Login form
    // Here we'll be taking a look at local storage

    function login(loginData) {
        // this will call /api/token
        var data = 'grant_type=password&username=' + loginData.username + '&password=' + loginData.password;

        var deferred = $q.defer();

        $http.post(apiUrl + 'token', data, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }

        }).success(function (response) {
            LocalStorageModule.set('token', {
                token: response.access_token
            });

            state.authorized = true;

            deferred.resolve(response);

        }).error(function (err, status) {
            logout();

            deferred.reject(err);
        });

        return deferred.promise;
    }
    
    function logout() {
        // this will log the user out
        LocalStorageModule.remove('token');

        state.authorized = false;
    }
    
        // The Revealing Module states that I have to define everything that I made one in my object or everything what my object does
        // And then I have to specify what's private and what's public
        // The public functions will be invited into the object that's about to be returned, while the private ones will not
        // i.e. In my factory I need to return an object
        // My object is going to have an initialize function, which refers to the function initialize() method, register, login, and logout
        // It also needs include state as an object that needs to be returned

    return {
        state: state,
        initialize: initialize,
        register: register,
        login: login,
        logout: logout
    };

        // We do use the Revealing Module Pattern in a different way in the interceptor, but this is the basics of it:
        // We define everything first
        // And then we wrap it all up into a nice object ( return {} )

});


        // NOTES:

        // I changed "localStorageService" to "LcoalStorageModule" -- it is written with uppercase "L" in app.js (top of page)

        // A service is like a re-useable chunk of code 
        // A controller interacts with html and the template view
        // $http - this service will allow me to make http requests
        // $q - a library that helps run functions asynchronously so you can use their return values once they finish processing

        // REVEALING MODULE PATTERN, CAM'S TAKE ON IT: 
        // JavaScript sucks at object oriented programming, there is no easy way to do encapsulation
        // so the revealing modular pattern is in my (Cam's) opinion the cleanest way to do encapsulation in JavaScript
        // it basically means you can make certain things private and certain things public

        // REVEALING MODULE PATTERN GOOGLE SEARCH RESULT:
        /* The Revealing Module Pattern is based on a pattern referred to as the Module Pattern. 
        It makes reading code easier and allows it to be organized in a more structured manner. 
        The pattern starts with code like the following to define a variable, associate it with 
        a function and then invoke the function immediately as the script loads. The final parenthesis
        shown in the code cause it to be invoked. */

        // var calculator = function () { /* Code goes here */ }();

        /* Variables and functions that should be encapsulated within the calculator object go in the
        “Code goes here” section. What's really nice about the pattern is that you can define which
        members are publicly accessible and which members are private. This is done by adding a return
        statement at the end of the function that exposes the public members. */

        /* The Revealing Module Pattern is currently my favorite pattern out there for structuring JavaScript
        code mainly because it's easy to use, very readable (which is important for maintenance), and provides
        a simple way to expose public members to consumers. */ 