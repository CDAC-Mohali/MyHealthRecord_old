/*******************************************************************************
 * Copyright 2014 Centre for Development of Advanced Computing(C-DAC), Pune
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 ******************************************************************************/
var index = 0;
var jsonData;
var currentCountDisplayed;
var displaySearch;
var h = 80;
var dialogFormHTML;
var divDialogForm;
var txtId;
var selectedConceptId;

var enumSERVICES = {
	SEARCH : {
		type : "search",
		suggestbyacceptability_url : "http://localhost:8080/csnoserv/api/search/suggestbyacceptability",
		searchbyacceptability_url : "http://localhost:8080/csnoserv/api/search/searchbyacceptability",

	},

};

var enumACCEPTABILITY = {
	ALL : "all",
	PREFER_TERM : "preferterm"
};
var enumSTATE = {
	ALL : "all",
	ACTIVE : "active",
	IN_ACTIVE : "inactive"
};

var enumSUFFIX = {

	ALL : "all",
	PROCEDURE : "procedure",
	DISORDER : "disorder",
	FINDING : "finding",
	OBSERVABLE_ENTITY : "observable entity",
	BODY_STRUCTURE : "body structure",
	ORGANISM : "organism",
	SUBSTANCE : "substance",
	SPECIMEN : "specimen",
	SPECIAL_CONCEPT : "special concept",
	LINKAGE_CONCEPT : "linkage concept",
	PHYSICAL_FORCE : "physical force",
	EVENT : "event",
	ENVIRONMENT : "environment",
	GEOGRAPHIC_LOCATION : "geographic location",
	SOCIAL_CONCEPT : "social concept",
	SITUATION_WITH_EXPLICIT_CONTEXT : "situation",
	STAGING_SCALE : "staging scale",
	PHYSICAL_OBJECT : "physical object",
	QUALIFIER_VALUE : "qualifier value",
	RECORD_ARTIFACT : "record artifact",
	PERSON : "person",
	LINK_ASSERTION : "link assertion",
	NAMESPACE_CONCEPT : "namespace concept",
	ATTRIBUTE : "attribute",
	ASSESSMENT_SCALE : "assessment scale",
	RACIAL_GROUP : "racial group",
	TUMOR_STAGING : "tumor staging",
	OCCUPATION : "occupation",
	MORPHOLOGIC_ABNORMALITY : "morphologic abnormality",
	CELL : "cell",
	CELL_STRUCTURE : "cell structure",
	ETHNIC_GROUP : "ethnic group",
	PRODUCT : "product",
	INACTIVE_CONCEPT : "inactive concept",
	NAVIGATIONAL_CONCEPT : "navigational concept",
	LIFE_STYLE : "life style",
	REGIME_THERAPY : "regime/therapy",
	RELIGION_PHILOSOPHY : "religion/philosophy"

};


$(document)
		.ready(
				function() {

					selectedConceptId = "";
					dialogFormHTML = '<div id="snomed-ct-search"><input type="text" placeholder="Enter 3 characters to search..." id="txt-snomed-ct-search" class="searchText" name="txt-snomed-ct-search" style="width:520px;color:#029cdb;"></input><input type="image" name="submit" src="images/search.png" id="srcImg" alt="Search"  title="Search" style="height: 20px; width: 20px;float:right;cursor:pointer;position:relative;top:3px;padding-left:5px;display:block;"></div>';
					dialogFormHTML += '<label id="reccnt">No. of records : </label><span id="reccount" vertical-align:top; ></span><label id="norec">No results found</label>';
					dialogFormHTML += '<div class="concept" id="conceptdiv">';
					dialogFormHTML += '<table class="bordered">';
					dialogFormHTML += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
					dialogFormHTML += '<tr><td>--</td><td>--</td></tr>';
					dialogFormHTML += '<tr><td>--</td><td>--</td></tr>';
					dialogFormHTML += '<tr><td>--</td><td>--</td></tr>';
					dialogFormHTML += '<tr><td>--</td><td>--</td></tr>';
					dialogFormHTML += '<tr><td>--</td><td>--</td></tr>';
					dialogFormHTML += '</table>';
					dialogFormHTML += ' </div>';
					dialogFormHTML += '<div id="licenseBox"></div>';
					divDialogForm = '<div id="dialog-form" style="width: 600px;height:500px;">'
							+ dialogFormHTML + '</div>';
				
					$('body').append(divDialogForm);
					
				
					
					dialogData = '';
					dialogHTML = '<div id="dialog-message" title="CSNOPlugin Information"><p>'
							+ dialogData + '</p></div>';
					$('body').append(dialogHTML);

					var dialog = $("#dialog-form").dialog({

						autoOpen : false,
						height : 500,
						width : 600,
						modal : true,

						buttons : {

							"." : function(event) {

							}
						}

					}

					);

					dialog.data("uiDialog")._title = function(title) {
						title.html(this.options.title);

					};

				
					if(document.getElementById("conceptdiv"))
						$("#conceptdiv").remove();
					
					dialog
							.dialog(
									'option',
									'title',
									'<img style="float: left; position: relative; vertical-align: text-bottom; height: 33px; width: 36px; top: 0px;" src="css/images/CDACLogo.png" />&nbsp;&nbsp;<img style="width:63px;height:36px;vertical-align: text-bottom;float:right;" src="css/images/SCtrl.png" style="position:relative;right:0px;top:0px;" />');
			
				
					

					
				});


/*
 * loadResultsList() function loads the matching records for the term searched in 
 * SNOMEDCT repository. The records contains the Concept-id,Description and Description-id   
 * The result will display the concept-id, description-id and description of the selected 
 * concept from the result list.
 * 
 * Parameter Description:
	@param state(var)-Defines state of the concept. Refer enumSTATE in searchtool.js to pass values for the state_in parameter
					If state is,
					ACTIVE- It will return all active Concepts with all active Descriptions.
					IN_ACTIVE- It will return all inactive Concepts with all active and inactive Descriptions.
					ALL- It will return all active and inactive Concepts with all active and inactive Descriptions.
					
	@param suffix(var)-Defines suffix in which SNOMEDT CT term/text will be searched. Refer enumSUFFIX in searchtool.js to pass values for the suffix_in parameter.
					If suffix is,
					ALL- It will search SNOMED CT term in all Suffices
					
					Other examples-'PROCEDURE','DISORDER','BODY_STRUCTURE'
					
	@param acceptability(var)-Represents whether only FSN and Prefer Terms are to be fetched or all matching terms.Refer enumACCEPTABILITY in searchtool.js to pass values for the acceptability_in parameter.
	 				If acceptability is,
					PREFER_TERM- Only FSN and Prefer Terms for given term will be fetched
					ALL- It will fetch all SNOMED CT terms
					
	@param hits(var)-Maximum number of matching terms to be fetched. To get all the hits available, pass '-1'.
 	@param call (var)-It will contain the definition of the callback function used to retrieve return value/result from the selectSNOMEDCT function.
					The result includes Concept-id,Description-id and Description of the selected concept from SNOMED CT repository.
					User will have to define a callback function in your HTML page like-
					
					var callback =function(selectedSNOMEDTerm){setValue(selectedSNOMEDTerm);};
					function setValue(selectedSNOMEDTerm)
					{
						//selectedSNOMEDTerm contains Concept-id,Description-id and Description. User can manipulate the string or edit the function definition as per his requirement
						//document.getElementById(HTMLcontrolid).value=selectedSNOMEDTerm;
					}
					
					The function shall contain only 1 parameter <selectedSNOMEDTerm> that refers to the result including Concept-id,Description-id and Description. 
					User will have to define a function like setValue in the callback function to set the return value <selectedSNOMEDTerm> in some HTML control of the web page.
					For different HTML controls user will have to define different callback functions and write corresponding <setValue> functions for them.
 * */

function loadResultsList(state, suffix, acceptability, hits,call)
{
	$('#reccnt').show();
	$('#reccount').show();
	$('#norec').hide();
	
	if(document.getElementById("conceptdiv"))
		$("#conceptdiv").remove();
	
	if (displaySearch == false) {
		
		
		if(document.getElementById("conceptdiv"))
			$("#conceptdiv").remove();
		displaySearch = true;
	}

	if (displaySearch == true) {

		var dataValue = document.getElementById("txt-snomed-ct-search").value;

		if (dataValue.trim() == '') {
		
			var divhtmlData = '';	
			divhtmlData += '<div class="concept" id="conceptdiv">';
			divhtmlData += '<table class="bordered">';
			divhtmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
			divhtmlData += '<tr><td>--</td><td>--</td></tr>';
			divhtmlData += '<tr><td>--</td><td>--</td></tr>';
			divhtmlData += '<tr><td>--</td><td>--</td></tr>';
			divhtmlData += '<tr><td>--</td><td>--</td></tr>';
			divhtmlData += '<tr><td>--</td><td>--</td></tr>';
			$("#dialog-form").dialog("option", "height", "500");
			$('#dialog-form').dialog({
				position : 'center'
			});
			divhtmlData += '</table>';
			divhtmlData += '</div>';
			if(document.getElementById("conceptdiv"))
				$("#conceptdiv").remove();
			$("#dialog-form").append(divhtmlData);
			
			$('#reccnt').hide();
			$('#reccount').hide();
			$('#norec').show();
			var txtBox = document.getElementById("txt-snomed-ct-search");
			txtBox.focus();
			return;
		}

		var servURL = '';

		servURL = enumSERVICES.SEARCH.searchbyacceptability_url;

		servURL += "?term=" + encodeURIComponent(dataValue) + "&state=" + state
				+ "&suffix=" + suffix + "&acceptability=" + acceptability
				+ "&hits=" + hits;
		
		$
				.ajax({
					type : "GET",
					url : servURL,
					dataType : "jsonp",
					crossDomain : true,
					success : function(data, textStatus, jqXHR) {

							var htmlData = '';
							jsonData = data;
							$('#reccount').text(data.length);	
							if (data.length == 0) {
								var divhtmlData = '';	
								divhtmlData += '<div class="concept" id="conceptdiv">';
								divhtmlData += '<table class="bordered">';
								divhtmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
								divhtmlData += '<tr><td>--</td><td>--</td></tr>';
								divhtmlData += '<tr><td>--</td><td>--</td></tr>';
								divhtmlData += '<tr><td>--</td><td>--</td></tr>';
								divhtmlData += '<tr><td>--</td><td>--</td></tr>';
								divhtmlData += '<tr><td>--</td><td>--</td></tr>';
								$("#dialog-form").dialog("option", "height", "500");
								$('#dialog-form').dialog({
									position : 'center'
								});
								divhtmlData += '</table>';
								divhtmlData += '</div>';
								if(document.getElementById("conceptdiv"))
									$("#conceptdiv").remove();
								$("#dialog-form").append(divhtmlData);
							
							$('#reccnt').hide();
							$('#reccount').hide();
							$('#norec').show();
							var txtBox = document
									.getElementById("txt-snomed-ct-search");
							txtBox.focus();
							return;
						}
						
						displaySearch = false;
						if (data.length <= 5) 
						{
							htmlData = '';	
							htmlData += '<div class="concept" id="conceptdiv">';
							
							htmlData += '<table class="bordered">';
							htmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
							currentCountDisplayed = data.length;
							for ( var i = 0; i < data.length; ++i) 
							{
								var val = '\'' + data[i].conceptid + '###$$$'
										+ data[i].description + '###$$$'
										+ data[i].descriptionid + '\'';

								htmlData += '<tr onclick="selectValue(\''
										+ escape(val) + '\','+call+');"><td>'
										+ data[i].conceptid
										+ '&nbsp;&nbsp;&nbsp;&nbsp;</td><td>'
										+ data[i].description + '</td></tr>';
							}
							$("#dialog-form").dialog("option", "height", "500");
							// false)
							// );
							$('#dialog-form').dialog({
								position : 'center'
							});
							htmlData += '</table>';
							htmlData += '</div>';
							if(document.getElementById("conceptdiv"))
								$("#conceptdiv").remove();
							$("#dialog-form").append(htmlData);
						} else {
							$("#dialog-form").dialog("option", "height", 500);
							$('#dialog-form').dialog({
								position : 'center'
							});
							index = 0;
							htmlData = '';
							htmlData += '<div class="concept" id="conceptdiv">';
							
							htmlData += '<table class="bordered">';
							htmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
							var currentLength = index + 5;
							if (currentLength > jsonData.length)
								currentLength = jsonData.length;
							currentCountDisplayed = currentLength;
							for (; index < currentLength; index++) {
								var val = '\'' + data[index].conceptid
										+ '###$$$' + data[index].description
										+ '###$$$' + data[index].descriptionid
										+ '\'';

								htmlData += '<tr  onclick="selectValue(\''
										+ escape(val) + '\','+call+');"><td>'
										+ data[index].conceptid
										+ '&nbsp;&nbsp;&nbsp;&nbsp;</td><td>'
										+ data[index].description
										+ '</td></tr>';
							}
							htmlData += '</table>';
							htmlData += '<br/><center><button  id="next" onclick="nextPage('+call+');" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">  Next  </button></center>';
							htmlData += '</div>';
							if(document.getElementById("conceptdiv"))
								$("#conceptdiv").remove();
							$("#dialog-form").append(htmlData);
							
							
						}
						
					},
					error : function(jqXHR, textStatus, errorThrown) 
					{
						console.log(textStatus);
					}
				});
	} 
	else 
	{
		$("#snomedct").click();
	}
	
}

/* selectSNOMEDCT function provides the search and suggest results for the term to be searched
 * from SNOMEDCT repository with the help of input parameters ( state_IN, suffix_IN, acceptability_IN, hits_IN, callback). 
 * The most matching terms from the SNOMEDCT repository are populated and displayed on the page for the 
 * searched term.   
 * Parameter Description:
	@param state_in(var)-Defines state of the concept. Refer enumSTATE in searchtool.js to pass values for the state_in parameter
					If state is,
					ACTIVE- It will return all active Concepts with all active Descriptions.
					IN_ACTIVE- It will return all inactive Concepts with all active and inactive Descriptions.
					ALL- It will return all active and inactive Concepts with all active and inactive Descriptions.
					
	@param suffix_in(var)-Defines suffix in which SNOMEDT CT term/text will be searched. Refer enumSUFFIX in searchtool.js to pass values for the suffix_in parameter.
					If suffix is,
					ALL- It will search SNOMED CT term in all Suffices
					
					Other examples-'PROCEDURE','DISORDER','BODY_STRUCTURE'
					
	@param acceptability_in(var)-Represents whether only FSN and Prefer Terms are to be fetched or all matching terms.Refer enumACCEPTABILITY in searchtool.js to pass values for the acceptability_in parameter.
	 				If acceptability is,
					PREFER_TERM- Only FSN and Prefer Terms for given term will be fetched
					ALL- It will fetch all SNOMED CT terms
					
	@param hits_in(var)-Maximum number of matching terms to be fetched. To get all the hits available, pass '-1'.
 	@param callback (var)-It will contain the definition of the callback function used to retrieve return value/result from the selectSNOMEDCT function.
					The result includes Concept-id,Description-id and Description of the selected concept from SNOMED CT repository.
					User will have to define a callback function in your HTML page like-
					
					var callback =function(selectedSNOMEDTerm){setValue(selectedSNOMEDTerm);};
					function setValue(selectedSNOMEDTerm)
					{
						//selectedSNOMEDTerm contains Concept-id,Description-id and Description. User can manipulate the string or edit the function definition as per his requirement
						//document.getElementById(HTMLcontrolid).value=selectedSNOMEDTerm;
					}
					
					The function shall contain only 1 parameter <selectedSNOMEDTerm> that refers to the result including Concept-id,Description-id and Description. 
					User will have to define a function like setValue in the callback function to set the return value <selectedSNOMEDTerm> in some HTML control of the web page.
					For different HTML controls user will have to define different callback functions and write corresponding <setValue> functions for them.
 * 
 * */

function selectSNOMEDCT(state_IN, suffix_IN, acceptability_IN, hits_IN,callback) {

	if (hits_IN <= -1 || hits_IN == '' || hits_IN == undefined || hits_IN == null) 
	{
		hits_IN = -1;
	}
	
	if (state_IN == -1 || state_IN == null || state_IN == '' || state_IN == undefined) 
	{
		state_IN = enumSTATE.ALL;
	}
			else

		state_IN = enumSTATE[state_IN];

	if (suffix_IN == -1 || suffix_IN == null || suffix_IN == undefined || suffix_IN == '') {
		suffix_IN = enumSUFFIX.ALL;
	} 
	else
		suffix_IN = enumSUFFIX[suffix_IN];

	if (acceptability_IN == -1 || acceptability_IN == null || acceptability_IN == undefined || acceptability_IN == '') {
		acceptability_IN = enumACCEPTABILITY.ALL;
	} 
	else
		acceptability_IN = enumACCEPTABILITY[acceptability_IN];
	
	
	$("#dialog-form")
	.click(
			function(e) {
				if(e.target.id=="srcImg")
					{
					
					if(document.getElementById("conceptdiv"))
						$("#conceptdiv").remove();
					loadResultsList(state_IN, suffix_IN, acceptability_IN, hits_IN,callback);
					}

			});

	selectedConceptId = "";
	var footer = '';
	footer += '<div id="footer"><div class="footer-nav">';
	footer += '<ul><li style="float: right;margin-left: -11px; position: relative;">SNOMED-CT Version:2014_01_31</li>';
	footer += '<li style="float: right;position: absolute;right: 60px;">CSNOCtrl&nbsp;&copy;Centre for Development of Advanced Computing</li>';
	footer += '<li style="float: right;position: absolute;right: 4px;"><a href="#" id="license">License</a></li>';
	footer += '</ul><div class="cl">&nbsp;</div></div></div>';

	
	displaySearch = false;

	$("#dialog-form").dialog("option", "height", 500);
	$('#dialog-form').dialog({
		position : 'center'
	});
	$("#dialog-form").html(dialogFormHTML);
	jQuery('.ui-dialog button:nth-child(1)').button('disable');
	$('.ui-dialog-buttonpane').find("button").show().filter(":contains('.')")
			.hide();
	if (!$("#footer").length) {
		$(".ui-dialog-buttonpane").append(footer);
	}
	$("#reccnt").hide();
	$('#reccount').hide();
	$('#norec').hide();
	
	var txtBox = document.getElementById("txt-snomed-ct-search");
	txtBox.focus();

	$("#dialog-form").dialog("open");
	$("#txt-snomed-ct-search").keyup(
			function(e){
				var textvalue=document.getElementById("txt-snomed-ct-search").value;
				if ((textvalue=='')||(textvalue.length<3)) {
					var divhtmlData = '';	
					divhtmlData += '<div class="concept" id="conceptdiv">';
					divhtmlData += '<table class="bordered">';
					divhtmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
					divhtmlData += '<tr><td>--</td><td>--</td></tr>';
					divhtmlData += '<tr><td>--</td><td>--</td></tr>';
					divhtmlData += '<tr><td>--</td><td>--</td></tr>';
					divhtmlData += '<tr><td>--</td><td>--</td></tr>';
					divhtmlData += '<tr><td>--</td><td>--</td></tr>';
					$("#dialog-form").dialog("option", "height", "500");
					$('#dialog-form').dialog({
						position : 'center'
					});
					divhtmlData += '</table>';
					divhtmlData += '</div>';
					if(document.getElementById("conceptdiv"))
						$("#conceptdiv").remove();
					$("#dialog-form").append(divhtmlData);
					$('#reccnt').hide();
					$('#reccount').hide();
					$('#norec').hide();
				}
			});
	
	
	var xhrRequest = null;
	$("#txt-snomed-ct-search")
			.autocomplete(
					{
						max : 20,
						minLength : 3,
						cacheLength : 1,
						scroll : true,
						width : 520,
						highlight : false,
						autoFocus : true,
						source : function(request, response) {

							var dataValue = document
									.getElementById("txt-snomed-ct-search").value;

							var servURL = "";
							if (dataValue == '') {

								var txtBox = document
										.getElementById("txt-snomed-ct-search");
								txtBox.focus();
								return;
							}

							if (dataValue.length >= 3) {
								if(document.getElementById("conceptdiv"))
									$("#conceptdiv").remove();
								loadResultsList(state_IN, suffix_IN, acceptability_IN, hits_IN,callback);	
							}

							servURL = enumSERVICES.SEARCH.suggestbyacceptability_url;
							servURL += "?term="
									+ encodeURIComponent(request.term)
									+ "&state=" + state_IN + "&suffix=" + suffix_IN
									+ "&acceptability=" + acceptability_IN
									+ "&hits=" + hits_IN;
							
							
							if (xhrRequest && xhrRequest.readystate != 4)
								xhrRequest.abort();
							xhrRequest = $
									.ajax({
										type : "GET",
										url : servURL,
										dataType : "jsonp",
										crossDomain : true,
										success : function(data, textStatus,
												jqXHR) {
											console.log(data);
											var items = data;
											
											response(items);
										},
										error : function(jqXHR, textStatus,
												errorThrown) {
											console.log(textStatus);

										}
									});
									
						},
						select : function(event, ui) {
							
								
								document.getElementById("txt-snomed-ct-search").value = ui.item.value;
								if(document.getElementById("conceptdiv"))
									$("#conceptdiv").remove();
							
								loadResultsList(state_IN, suffix_IN, acceptability_IN, hits_IN,callback);
						}
						

					});
	$('#license').click(function() {
		$('#licenseBox').load("license.html", function(content) {
			$('#licenseBox').dialog({
				autoOpen : false,
				resizable : false,
				height : "auto",
				width : 800,
				dialogClass : 'noTitle',
				title : "LICENSE INFO",
				modal : true,
				show : {
					effect : "blind",
					duration : 1000
				},
				hide : {
					effect : "explode",
					duration : 1000
				}
			});
			$('#licenseBox').dialog('open');
		});

	});

}


/*
 * selectValue() function selects the record from the list. The record returns 
 * the details of the searched term from the SNOMEDCT repository. The details
 * include description id,description and concept id of the selected record. 
 * The result shows concept-id,description-id and description of the term.
 * @param value (var) contains the result including concept-id,
 * description-id and description.
 * 
 * @param callback (var) - It will contain the definition of the callback function used to retrieve
 * return value/result from the selectSNOMEDCT().The result includes Concept-id,Description-id and Description
 * of the selected concept. 
 *    
 * returnterm_OUT (var) contains the detailed output with concept-id,
 * description-id and description.
*/


function selectValue(value,callback) {
	
	
	var data = unescape(value);

	var term = data.split('###$$$');
	
	var returnterm_OUT = "Description :" + term[1]
					+ "\r\n" + "Concept Id:" + term[0].replace("'", "") + "\r\n"
					+ "Description id:" + term[2].replace("'", "");
	
	
	if (typeof callback === "function")
		callback(returnterm_OUT);
	
	$("#dialog-form").dialog("close");
	
}

/*
 * nextPage() is called when the user clicks on the Next button
 *
 * @param call (var) - It will contain the definition of the callback function used to retrieve
 * return value/result from the selectSNOMEDCT().The result includes Concept-id,Description-id and Description
 * of the selected concept. 
 * 
 * Use "Next" button to navigate the results in forward direction.
 * The result will display concept-id, description-id and description
 * of the selected record from the list.
 * */

function nextPage(call) {
	
	if(document.getElementById("conceptdiv"))
		$("#conceptdiv").remove();
	var htmlData = '';
	htmlData += '<div class="concept" id="conceptdiv">';
	htmlData += '<br/>';
	htmlData += '<table class="bordered">';
	htmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
	var currentLength = index + 5;
	if (currentLength > jsonData.length) {
		currentLength = jsonData.length;
	}
	currentCountDisplayed = currentLength - index;
	for ( var i = 0; index < currentLength; index++, i++) {
		var val = '\'' + jsonData[index].conceptid + '###$$$'
				+ jsonData[index].description + '###$$$'
				+ jsonData[index].descriptionid + '\'';
		htmlData += '<tr onclick=" selectValue(\'' + escape(val) + '\','+call+');"><td>'
				+ jsonData[index].conceptid
				+ '&nbsp;&nbsp;&nbsp;&nbsp;</td><td>'
				+ jsonData[index].description + '</td></tr>';
	}
	$("#dialog-form").dialog("option", "height", 500);
	$('#dialog-form').dialog({
		position : 'center'
	});
	htmlData += '</table>';
	htmlData += '<br/><center><button    id="previous" onclick="previousPage('+call+');" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">  Previous  </button> ';
	if (currentLength != jsonData.length) {
		htmlData += ' <button   id="next" onclick="nextPage('+call+');" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">  Next  </button> ';
	}
	htmlData += '</center> </div>';
	if(document.getElementById("conceptdiv"))
		$("#conceptdiv").remove();
	$("#dialog-form").append(htmlData);
	
}

/*
 * previousPage() is called when the user clicks on the Previous button
 * 
 * *
 * @param call (var) - It will contain the definition of the callback function used to retrieve
 * return value/result from the selectSNOMEDCT().The result includes Concept-id,Description-id and Description
 * of the selected concept. 
 * 
 * Use "Previous" button to navigate the results in backward direction.
 * Click on the record to get the concept-id, description-id and description
 * */

function previousPage(call) {
	if(document.getElementById("conceptdiv"))
		$("#conceptdiv").remove();
	var htmlData = '';
	htmlData += '<div class="concept" id="conceptdiv">';
	htmlData += '<br/>';
	htmlData += '<table class="bordered">';
	htmlData += '<thead><tr><th>Concept ID&nbsp;&nbsp;</th><th>Description</th></tr></thead>';
	index = index - currentCountDisplayed - 5;
	var currentLength = index + 5;
	currentCountDisplayed = currentLength - index;
	if (index < 0)
		index = 0;
	for (; index < currentLength; index++) {
		var val = '\'' + jsonData[index].conceptid + '###$$$'
				+ jsonData[index].description + '###$$$'
				+ jsonData[index].descriptionid + '\'';
		htmlData += '<tr onclick="selectValue(\'' + escape(val) + '\','+call+');"><td>'
				+ jsonData[index].conceptid
				+ '&nbsp;&nbsp;&nbsp;&nbsp;</td><td>'
				+ jsonData[index].description + '</td></tr>';
	}
	htmlData += '</table>';

	if (currentLength - currentCountDisplayed != 0) {
		htmlData += '<br/><center><button   id="previous" onclick="previousPage('+call+');" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">  Previous  </button>';
	} else
		htmlData += '<br/><center>';

	htmlData += '  <button   id="next" onclick="nextPage('+call+');" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">  Next  </button></center>';
	htmlData += ' </div>';
	$("#dialog-form").dialog("option", "height", 500);
	$('#dialog-form').dialog({
		position : 'center'
	});
	if(document.getElementById("conceptdiv"))
		$("#conceptdiv").remove();
	$("#dialog-form").append(htmlData);
	
}

function message(val) {
	dialogHTML = '<p>' + val + '</p>';
	$("#dialog-message").html(dialogHTML);

	$("#dialog-message").dialog({
		modal : true,
		buttons : {
			Ok : function() {
				$("#txt-snomed-ct-search").focus();
				$("#dialog-message").dialog("close");
			}
		}
	});
}

function getHeight(count, buttonPlace) {
	if (buttonPlace == true) {
		switch (count) {
		case 1:
			return 270;
		case 2:
			return 320;
		case 3:
			return 370;
		case 4:
			return 410;
		case 5:
			return 450;
		}
	} else {
		switch (count) {
		case 1:
			return 250;
		case 2:
			return 300;
		case 3:
			return 350;
		case 4:
			return 390;
		case 5:
			return 420;
		}
	}
}
