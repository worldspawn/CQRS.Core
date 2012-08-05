var yehnah = {};
yehnah.utils = new function () {
  //http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript
  this.createUUID = function () {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  };

  this.prepareCommand = function (command) {
    var payload = ko.mapping.toJS(command);
    $.extend(payload, { commandId: this.createUUID() });
    return payload;
  }
} ();