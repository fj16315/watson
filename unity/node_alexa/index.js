var express = require('express')
    ,app = express()
    ,last_value
    ,greeting;

app.set('port', (process.env.PORT || 5000));

app.get('/', function (req, res) {
	console.log("Command = ", req.query.command);
	console.log("Greeting = ", req.query.greeting);
  if(req.query.command == undefined || req.query.command == ""){
	res.send("{ \"command\":\"" + last_value + "\"}\n{ \"greeting\":\"" + greeting + "\"}");
  }else{
	if(req.query.command == "empty"){
		last_value = "";
		res.send("{}");
	}else{
		last_value = req.query.command;
		greeting = req.query.greeting;
		res.send("{ \"command\":\"" + last_value + "\"}\n{ \"greeting\":\"" + greeting + "\"}");
	}
  }
})

app.listen(app.get('port'), function () {
  console.log("Node app is running on port", app.get('port'));
})