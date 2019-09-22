(function (speak) {
    var parentApp = window.parent.Sitecore.Speak.app.findApplication('EditActionSubAppRenderer');

    speak.pageCode(["underscore"],
        function (_) {
            return {
                initialized: function () {
                    this.on({
                        "loaded": this.loadDone
                    },
                        this);

                    if (parentApp) {
                        parentApp.loadDone(this, this.HeaderTitle.Text, this.HeaderSubtitle.Text);
                        parentApp.setSelectability(this, true);
                    }
                },

                loadDone: function (parameters) {
                    this.Parameters = parameters || {};
                },

                getData: function () {
                    var formData = this.SetAreaForm.getFormData(),
                        keys = _.keys(formData);

                    keys.forEach(function (propKey) {
                        if (formData[propKey] == null || formData[propKey].length === 0) {
                            if (this.Parameters.hasOwnProperty(propKey)) {
                                delete this.Parameters[propKey];
                            }
                        } else {
                            this.Parameters[propKey] = formData[propKey];
                        }
                    }.bind(this));

                    return this.Parameters;
                }
            };
        });
})(Sitecore.Speak);