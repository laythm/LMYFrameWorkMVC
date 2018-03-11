//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    var LMYFrameWork = {};
//this is a static class because signalr is connected once only
LMYFrameWork.SignalRWrapper = {
    init: function (sessionID) {
        var context = this;

        //context.Started = false;

        context.appHub = $.connection.appHub;

        if (!context.started()) {
            context.appHub.client.connected = function (isConnected) {
            };

            context.appHub.client.disconnected = function () {

            };

            context.appHub.client.OnReconnected = function (isConnected) {

            };
            //    $.connection.hub.logging = true;

            context.statrtHub(sessionID);

            context.initClientMethods();
        }

        $.connection.hub.stateChanged(function (change) {
            //{connecting: 0, connected: 1, reconnecting: 2, disconnected: 4}
            if (change.newState === $.signalR.connectionState.reconnecting) {
                context.executeConnectionStatusChangedCallBacks(false);
                console.log("signalr is reconnecting!");
            }
            else if (change.newState === $.signalR.connectionState.connected) {
                console.log("signalr is connected!");
                context.executeConnectionStatusChangedCallBacks(true);
            }
            else if (change.newState === $.signalR.connectionState.disconnected) {
                context.executeConnectionStatusChangedCallBacks(false);

                setTimeout(function () { context.statrtHub(sessionID); }, 200);
            }
        });
    },
    statrtHub: function (sessionID) {
        $.connection.hub.qs = { 'SessionID': sessionID }
        $.connection.hub.start({ transport: ['webSockets', 'foreverFrame', 'serverSentEvents', 'longPolling'] }).done(function () {

        }).fail(function () {

        });;
    },

    started: function () {
        // {connecting: 0, connected: 1, reconnecting: 2, disconnected: 4}
        return this.appHub.connection.state === $.signalR.connectionState.connected;
    },

    handleSuccess: function (successCallBack, errorCallBack, alertsContainer, data) {
        var context = this;
        //if no data or it does not has an error 
        if (!data || !data.HasError) {
            if (successCallBack)
                successCallBack(data);
        } else {
            if (errorCallBack)
                errorCallBack(data);
        }

        if (alertsContainer)
            LMYFrameWork.CommonMethods.handleAlerts(data, alertsContainer);

    },

    handleError: function (successCallBack, errorCallBack, alertsContainer, data) {
        var context = this;

        if (errorCallBack)
            errorCallBack(data, LMYFrameWork.Constants.jsonType);

        if (alertsContainer)
            LMYFrameWork.CommonMethods.handleAlerts(data, alertsContainer);
    },

    addCallBackToConnectionStatusChanged: function (callBack, unBindOnConnected) {
        var context = this;

        if (!context.ConnectionStatusChangedCallBacks)
            context.ConnectionStatusChangedCallBacks = [];

        context.ConnectionStatusChangedCallBacks.push({ callBack: callBack, unBindOnConnected: unBindOnConnected });
    },

    executeConnectionStatusChangedCallBacks: function (isConnected) {
        var context = this;

        var arrUnbindIndexes = [];
        if (context.ConnectionStatusChangedCallBacks)
            for (i = 0; i < context.ConnectionStatusChangedCallBacks.length; i++) {
                context.ConnectionStatusChangedCallBacks[i].callBack(isConnected);

                if (isConnected)
                    if (context.ConnectionStatusChangedCallBacks[i].unBindOnConnected)
                        arrUnbindIndexes.push(i);
            }
        //to remove all call backs that sent the parameter unBindOnConnected as true
        for (i = 0; i < arrUnbindIndexes.length; i++) {
            context.ConnectionStatusChangedCallBacks.splice(arrUnbindIndexes[i], 1);
        }
    },
 
    initClientMethods: function () {
        var context = this;
        //client methods
        context.appHub.client.RecieveMessage = function (data) {
            if (context.RecieveMessageCallBacks)
                for (i = 0; i < context.RecieveMessageCallBacks.length; i++) {
                    context.RecieveMessageCallBacks[i](data);
                }
        };

        context.appHub.client.ReadMessageUpdated = function (data) {
            if (context.ReadMessageUpdatedCallBacks)
                for (i = 0; i < context.ReadMessageUpdatedCallBacks.length; i++) {
                    context.ReadMessageUpdatedCallBacks[i](data);
                }
        };

        context.appHub.client.UserIsOnline = function (userid, isOnline) {
            if (context.UserIsOnlineCallBacks)
                for (i = 0; i < context.UserIsOnlineCallBacks.length; i++) {
                    context.UserIsOnlineCallBacks[i](userid, isOnline);
                }
        };

        context.appHub.client.MessageSentSuccessfully = function (message) {
            if (context.MessageSentSuccessfullyCallBacks)
                for (i = 0; i < context.MessageSentSuccessfullyCallBacks.length; i++) {
                    context.MessageSentSuccessfullyCallBacks[i](message);
                }
        };

        context.appHub.client.MessageSetAsReadSuccessfully = function (messageId) {
            if (context.MessageSetAsReadSuccessfullyCallBacks)
                for (i = 0; i < context.MessageSetAsReadSuccessfullyCallBacks.length; i++) {
                    context.MessageSetAsReadSuccessfullyCallBacks[i](messageId);
                }
        };

        context.appHub.client.AutomaticLogOut = function () {
            if (context.AutomaticLogOutLogOutCallBacks)
                for (i = 0; i < context.AutomaticLogOutLogOutCallBacks.length; i++) {
                    context.AutomaticLogOutLogOutCallBacks[i]();
                }
        };

        context.appHub.client.UpdateNotReadCount = function (count) {
            if (context.UpdateNotReadCountCallBacks)
                for (i = 0; i < context.UpdateNotReadCountCallBacks.length; i++) {
                    context.UpdateNotReadCountCallBacks[i](count);
                }
        };
    },

    addCallBackToRecieveMessage: function (callBack) {
        var context = this;

        if (!context.RecieveMessageCallBacks)
            context.RecieveMessageCallBacks = [];

        context.RecieveMessageCallBacks.push(callBack);
    },

    addCallBackToReadMessageUpdated: function (callBack) {
        var context = this;

        if (!context.ReadMessageUpdatedCallBacks)
            context.ReadMessageUpdatedCallBacks = [];

        context.ReadMessageUpdatedCallBacks.push(callBack);
    },

    addCallBackToUserIsOnline: function (callBack) {
        var context = this;

        if (!context.UserIsOnlineCallBacks)
            context.UserIsOnlineCallBacks = [];

        context.UserIsOnlineCallBacks.push(callBack);
    },

    addCallBackToMessageSentSuccessfully: function (callBack) {
        var context = this;

        if (!context.MessageSentSuccessfullyCallBacks)
            context.MessageSentSuccessfullyCallBacks = [];

        context.MessageSentSuccessfullyCallBacks.push(callBack);
    },

    addCallBackToMessageSetAsReadSuccessfully: function (callBack) {
        var context = this;

        if (!context.MessageSetAsReadSuccessfullyCallBacks)
            context.MessageSetAsReadSuccessfullyCallBacks = [];

        context.MessageSetAsReadSuccessfullyCallBacks.push(callBack);
    },

    addCallBackAutomaticLogOut: function (callBack) {
        var context = this;

        if (!context.AutomaticLogOutLogOutCallBacks)
            context.AutomaticLogOutLogOutCallBacks = [];

        context.AutomaticLogOutLogOutCallBacks.push(callBack);
    },

    addCallBackToUpdateNotReadCount: function (callBack) {
        var context = this;

        if (!context.UpdateNotReadCountCallBacks)
            context.UpdateNotReadCountCallBacks = [];

        context.UpdateNotReadCountCallBacks.push(callBack);
    },

    sendMessage: function (toUserID, messageText, settings) {

        options = {
            successCallBack: '',
            errorCallBack: '',
            alertsContainer: ''
        }
        $.extend(options, settings);

        var context = this;
        if (context.started()) {
            context.appHub.server.sendMessage(toUserID, messageText).done(function (data) {
                context.handleSuccess(options.successCallBack, options.errorCallBack, options.alertsContainer, data);
            }).fail(function (data) {
                context.addCallBackToConnectionStatusChanged(function (isConnected) {
                    if (isConnected) {
                        context.sendMessage(toUserID, messageText, settings);;
                    }

                }, true);
                //setTimeout(function () { context.sendMessage(toUserID, messageText, settings);; }, 100);
                // context.handleError(options.successCallBack, options.errorCallBack, options.alertsContainer, data);
            });
        } else {
            //if not connected then retry to connect 
            //after reconnect call this function again
            context.addCallBackToConnectionStatusChanged(function (isConnected) {
                if (isConnected) {
                    context.sendMessage(toUserID, messageText, settings);;
                }
            }, true);
        }
    },

    setMessageAsRead: function (id, settings) {
        options = {
            successCallBack: '',
            errorCallBack: '',
            alertsContainer: ''
        }
        $.extend(options, settings);

        var context = this;
        if (context.started()) {
            context.appHub.server.setMessageAsRead(id).done(function (data) {
                context.handleSuccess(options.successCallBack, options.errorCallBack, options.alertsContainer, data);
            }).fail(function (data) {
                context.addCallBackToConnectionStatusChanged(function (isConnected) {
                    if (isConnected) {
                        context.setMessageAsRead(id, settings);
                    }
                }, true);
            });
        }
        else {
            //if not connected then retry to connect 
            context.addCallBackToConnectionStatusChanged(function (isConnected) {
                if (isConnected) {
                    context.setMessageAsRead(id, settings);
                }
            }, true);
        }
    },

    fireUpdateNotReadCountForCurrentConnection: function (settings) {
        options = {
            successCallBack: '',
            errorCallBack: '',
            alertsContainer: ''
        }
        $.extend(options, settings);

        var context = this;
        if (context.started()) {
            context.appHub.server.fireUpdateNotReadCountForCurrentConnection().done(function () {
                context.handleSuccess(options.successCallBack, options.errorCallBack, options.alertsContainer);
            }).fail(function (data) {
                context.addCallBackToConnectionStatusChanged(function (isConnected) {
                    if (isConnected) {
                        context.fireUpdateNotReadCountForCurrentConnection();
                    }
                }, true);

                //context.handleError(options.successCallBack, options.errorCallBack, options.alertsContainer, data);
            });
        }
        else {
            //if not connected then retry to connect 
            //after reconnect call this function again
            context.addCallBackToConnectionStatusChanged(function (isConnected) {
                if (isConnected) {
                    context.fireUpdateNotReadCountForCurrentConnection();
                }
            }, true);
            // setTimeout(function () { context.setMessageAsRead(id, settings); }, 500);
        }
    }
}