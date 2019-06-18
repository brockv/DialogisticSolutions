/* This script handles model validation within a modal */

/* Ensure that the document is fully loaded before allowing access to this */
$(function () {
    /* Grab the div that the modal will be displayed in */
    var placeholderElement = $('#modal-placeholder');
    var obj = {};

    /* Listen for the button that opens the modal to be pressed */
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        /* Grab the url we'll use to make the GET request, which is defined in the button */
        var url = $(this).data('url');

        /* Make the GET request in order to retrieve and display the correct information in the modal */
        $.get(url).done(function (data) {
            /* Fill out the modal's elements with the data that was retrieved, and then show the modal */
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    /* Listen for the submit button in the modal to be pressed */
    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        /* Prevent the modal's form from being submitted normally */
        event.preventDefault();

        /* Get the form inside the modal, and grab the destination url that we'll make the POST request at */
        var form = $(this).parents('.modal').find('form');
        var controllerUrl = form.attr('controller');
        var actionUrl = form.attr('action');
        var fullUrl = controllerUrl + "/" + actionUrl;
        /* Serialize the form data so that we can send all of it to the controller */
        var dataToSend = form.serialize();

        /* Parse the serialized form data so that we can pull values from it easier */
        var data = dataToSend.split("&");
        for (var key in data) {
            /* Grab each key/value pair and store them */
            obj[data[key].split("=")[0]] = data[key].split("=")[1];
        }


        /* Make the POST request in the background, keeping the modal open */
        $.post(fullUrl, dataToSend).done(function (data) {
            /* Rebuild the contents of the modal using the data sent back from the controller */
            var newBody = $('.modal-body', data);
            jQuery.noConflict();

            /* If the model state came back invalid, redisplay the modal with the validation error 
             * messages, otherwise display a success message and close the modal */
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (!isValid) {
                placeholderElement.find('.modal-body').replaceWith(newBody);
                reloadLists(data);                
            } else {
                alert("Changes submitted successfully.");
                $("#updateInformation").modal('hide');
            }
        });
    });

    function reloadLists() {
        function ajaxCall() {
            this.send = function (data, url, method, success, type) {
                type = type || 'json';
                var successRes = function (data) {
                    success(data);
                }

                var errorRes = function (e) {
                    console.log(e);
                }
                jQuery.ajax({
                    url: url,
                    type: method,
                    data: data,
                    success: successRes,
                    error: errorRes,
                    dataType: type,
                    timeout: 60000
                });

            }

        }

        function locationInfo() {
            var rootUrl = "//geodata.solutions/api/api.php";
            //set default values
            var username = 'demo';
            var ordering = 'name';
            //now check for set values
            var addParams = '';
            if (jQuery("#gds_appid").length > 0) {
                addParams += '&appid=' + jQuery("#gds_appid").val();
            }
            if (jQuery("#gds_hash").length > 0) {
                addParams += '&hash=' + jQuery("#gds_hash").val();
            }

            var call = new ajaxCall();

            this.confCity = function (id) {
                var url = rootUrl + '?type=confCity&countryId=' + jQuery('#countryId').val() + '&stateId=' + jQuery('#stateId option:selected').attr('stateid') + '&cityId=' + id;
                var method = "post";
                var data = {};
                call.send(data, url, method, function (data) {
                    if (data) {
                        //    alert(data);
                    }
                    else {
                        //   alert('No data');
                    }
                });
            };

            this.getCities = function (id) {
                jQuery(".cities option:gt(0)").remove();
                //get additional fields
                var stateClasses = jQuery('#cityId').attr('class');
                //console.log(stateClasses);
                var cC = stateClasses.split(" ");
                cC.shift();
                var addClasses = '';
                if (cC.length > 0) {
                    acC = cC.join();
                    addClasses = '&addClasses=' + encodeURIComponent(acC);
                }
                var url = rootUrl + '?type=getCities&countryId=' + jQuery('#countryId').val() + '&stateId=' + id + addParams + addClasses;
                var method = "post";
                var data = {};
                jQuery('.cities').find("option:eq(0)").html("Please wait..");
                call.send(data, url, method, function (data) {
                    jQuery('.cities').find("option:eq(0)").html("Select City");
                    if (data.tp == 1) {
                        if (data.hits > 1000) {
                            //alert('Free usage far exceeded. Please subscribe at geodata.solutions.');
                            console.log('Daily geodata.solutions request limit exceeded:' + data.hits + ' of 1000');
                        }
                        else {
                            console.log('Daily geodata.solutions request count:' + data.hits + ' of 1000')
                        }

                        var listlen = Object.keys(data['result']).length;
                        //console.log('number is cities is ' + listlen);
                        if (listlen > 0) {
                            jQuery.each(data['result'], function (key, val) {

                                var option = jQuery('<option />');
                                option.attr('value', val).text(val);
                                jQuery('.cities').append(option);
                            });
                        }
                        else {
                            var usestate = jQuery('#stateId option:selected').val();
                            var option = jQuery('<option />');
                            option.attr('value', usestate).text(usestate);
                            option.attr('selected', 'selected');
                            jQuery('.cities').append(option);
                        }

                        jQuery(".cities").prop("disabled", false);

                        $('#cityId').val(obj.PreferredCity).attr("selected", "selected");
                    }
                    else {
                        alert(data.msg);
                    }
                });
            };

            this.getStates = function (id) {
                jQuery(".states option:gt(0)").remove();
                jQuery(".cities option:gt(0)").remove();
                //get additional fields
                var stateClasses = jQuery('#stateId').attr('class');
                console.log(stateClasses);
                var cC = stateClasses.split(" ");
                cC.shift();
                var addClasses = '';
                if (cC.length > 0) {
                    acC = cC.join();
                    addClasses = '&addClasses=' + encodeURIComponent(acC);
                }
                var url = rootUrl + '?type=getStates&countryId=' + id + addParams + addClasses;
                var method = "post";
                var data = {};
                jQuery('.states').find("option:eq(0)").html("Please wait..");
                call.send(data, url, method, function (data) {
                    jQuery('.states').find("option:eq(0)").html("Select State");
                    if (data.tp == 1) {
                        if (data.hits > 1000) {
                            console.log('Daily geodata.solutions request limit exceeded: ' + data.hits + ' of 1000.');
                        }
                        else {
                            console.log('Daily geodata.solutions request count:' + data.hits + ' of 1000')
                        }
                        jQuery.each(data['result'], function (key, val) {
                            var option = jQuery('<option />');
                            option.attr('value', val).text(val);
                            option.attr('stateid', key);
                            jQuery('.states').append(option);
                        });
                        jQuery(".states").prop("disabled", false);                       

                        $('#stateId').val(obj.PreferredState).attr("selected", "selected");

                        var loc = new locationInfo();
                        var stateId = jQuery("option:selected", ".states").attr('stateid');
                        if (stateId != '') {
                            loc.getCities(stateId);
                        }

                    }
                    else {
                        alert(data.msg);
                    }
                });
            };
        }

        jQuery(function () {
            var loc = new locationInfo();
            var coid = jQuery("#countryId").val();
            loc.getStates(coid);
            jQuery(".states").on("change", function (ev) {
                var stateId = jQuery("option:selected", this).attr('stateid');
                if (stateId != '') {
                    loc.getCities(stateId);
                }
                else {
                    jQuery(".cities option:gt(0)").remove();
                }
            });
            jQuery(".cities").on("change", function (ev) {
                var cityId = jQuery("option:selected", this).val();
                if (cityId != '') {
                    loc.confCity(cityId);
                }
            });
        });
    }
});