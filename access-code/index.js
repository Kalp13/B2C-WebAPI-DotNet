const portNumb = 80;

const express = require('express');
    let bodyParser = require('body-parser')
    //Create an express instance
    const app = express();

    app.use( bodyParser.json() );       // to support JSON-encoded bodies
    app.use(bodyParser.urlencoded({     // to support URL-encoded bodies
      extended: true
    }));


    app.post('/validate-accesscode', (req, res) => {
        let accessCode = '88888';
        if(accessCode == req.body.accessCode){
            res.status(200).send();
        }else{
            let errorResponse = {
                "version" :"1.0",
                "status" : 409,
                "code" : "errorCode",
                "requestId": "requestId",
                "userMessage" : "The access code you entered is incorrect. Please try again.",
                "developerMessage" : `The provided code ${req.body.accessCode} does not match the expected code for user.`,
                "moreInfo" :"https://docs.microsoft.com/en-us/azure/active-directory-b2c/string-transformations"
            };
            res.status(409).send(errorResponse);                
        }
    });

    app.listen(portNumb, () => {
        console.log(`Access code service listening on port !` + portNumb);
    });