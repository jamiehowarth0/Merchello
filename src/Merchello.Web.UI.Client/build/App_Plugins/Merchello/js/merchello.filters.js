/*! merchello
 * https://github.com/meritage/Merchello
 * Copyright (c) 2015 Merchello;
 * Licensed MIT
 */

(function() { 

    /**
     * @ngdoc filter
     * @name greaterthan
     * @function
     * 
     * @description
     * filter where value is greater than specified value
     */
    var greaterthan = function() {
            return function (inputArr, propName, minValue) {
                var filterResult = [];
                angular.forEach(inputArr, function(inputItem) {
                if (inputItem[propName] > minValue) {
                    filterResult.push(inputItem);
                }
            });

            return filterResult;
	    };
    };

    angular.module('merchello.filters').filter('greaterthan', greaterthan);



    /**
     * @ngdoc filter
     * @name startfrom
     * @function
     * 
     * @description
     * We already have a limitTo filter built-in to angular,
     * let's make a startFrom filter 
     * Ref: http://jsfiddle.net/2ZzZB/56/
     */
    var startfrom = function() {
        return function(input, start) {
            start = +start; //parse to int
            return input.slice(start);
        };
    };

    angular.module('merchello.filters').filter('startfrom', startfrom);



})();