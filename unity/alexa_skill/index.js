var request = require("request")
    , AlexaSkill = require('./AlexaSkill')
    , APP_ID     = process.env.APP_ID;

var error = function (err, response, body) {
    console.log('ERROR [%s]', err);
};

//TODO: Change the contents of this function to handle our questions
var getJsonFromUnity = function(color, shape, callback){

  var command = "create " + color + " " + shape;

  if(color == "thank you"){
  	callback("thank you");
  }else{
    var options = 
    { method: 'GET',
      url: 'http://brass-monkey-watson.herokuapp.com/',
      qs: { command: command },
      headers: 
        { 'postman-token': '230914f7-c478-4f13-32fd-e6593d8db4d1',
          'cache-control': 'no-cache' }
    };

    var error_log = "";

    request(options, function (error, response, body) {
  	  if (!error) {
  	    error_log = color + " " + shape;
  	  }else{
  		  error_log = "There was a mistake";
  	  }
  		callback(error_log);
    });
  }
};

//TODO: Change the contents of this function to handle our questions
var handleUnityRequest = function(intent, session, response){
  getJsonFromUnity(intent.slots.color.value,intent.slots.shape.value,function(data){
	  if(data != "thank you"){
	    var text = 'The ' + data + ' has been created';
	    var reprompt = 'Which shape would you like?';
      response.ask(text, reprompt);
	  }else{
		  response.tell("You're welcome");
	  }
    }
  );
};

var Unity = function(){
  AlexaSkill.call(this, APP_ID);
};

Unity.prototype = Object.create(AlexaSkill.prototype);
Unity.prototype.constructor = Unity;

Unity.prototype.eventHandlers.onSessionStarted = function(sessionStartedRequest, session){
  console.log("onSessionStarted requestId: " + sessionStartedRequest.requestId
      + ", sessionId: " + session.sessionId);
};

Unity.prototype.eventHandlers.onLaunch = function(launchRequest, session, response){
  // This is when they launch the skill but don't specify what they want.

  //TODO: Needs changing
  var output = 'Welcome to Unity. Create any color shape by saying create and providing a color and a shape'; 
  //TODO: Needs changing
  var reprompt = 'Which shape would you like?';

  response.ask(output, reprompt);

  console.log("onLaunch requestId: " + launchRequest.requestId
      + ", sessionId: " + session.sessionId);
};

Unity.prototype.intentHandlers = {
  GetUnityIntent: function(intent, session, response){
    handleUnityRequest(intent, session, response);
  },

  HelpIntent: function(intent, session, response){
    //TODO: Needs changing
    var speechOutput = 'Create a new colored shape. Which shape would you like?';
    response.ask(speechOutput);
  }
};

exports.handler = function(event, context) {
    var skill = new Unity();
    skill.execute(event, context);
};
