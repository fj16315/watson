const request = require("request");
var AlexaSkill = require('./AlexaSkill')
    , APP_ID     = process.env.APP_ID;

var error = function (err, response, body) {
    console.log('ERROR [%s]', err);
};

//TODO: Change the contents of this function to handle our questions
var getJsonFromUnity = function(query, callback){

  var options = 
  { method: 'GET',
    url: 'http://brass-monkey-watson.herokuapp.com',
    qs: { command: query }
    headers: 
      { 'cache-control': 'no-cache' }
  };

  var error_log = "";

  request(options, function (error, response, body) {
    if (!error) {
      error_log = query;
    }else{
      error_log = "There was a mistake";
    }
    callback(error_log);
  });
};

var handleUnityRequest = function(intent, session, response){
  getJsonFromUnity(intent.slots.query.value, function(data){
    var text = data + ' has been asked';
    var reprompt = 'What would you like to ask?';
    response.ask(text, reprompt);
  });
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
  var output = 'Welcome to Watson. Approach a character and ask them a question. Make sure to use their name at the start.'; 
  //TODO: Needs changing
  var reprompt = 'What would you like to ask?';

  response.ask(output, reprompt);

  console.log("onLaunch requestId: " + launchRequest.requestId
      + ", sessionId: " + session.sessionId);
};

Unity.prototype.intentHandlers = {
  AiQueryIntent: function(intent, session, response){
    handleUnityRequest(intent, session, response);
  }
};

exports.handler = function(event, context) {
    var skill = new Unity();
    skill.execute(event, context);
};
