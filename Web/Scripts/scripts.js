// THIS MUST BE LEFT AT THE TOP OF THE PAGE...all links and scripts below will need this extension method
// Create an extension method ("resolve")
// that returns the correct server path of the server, whether the server is virtual directory or root
// see http://robertcorvus.com/easy-way-to-get-mvc-root-path-in-javascript/
Url = function() { };
Url.prototype = {
    _relativeRoot: '<%= ResolveUrl("~/") %>',
    // create an extension method called "resolve"
    resolve: function(relative) {
        var resolved = relative;
        if (relative.charAt(0) == '~')
            resolved = this._relativeRoot + relative.substring(2);
        return resolved;
    }
};
$Url = new Url();