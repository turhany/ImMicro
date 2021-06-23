using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IdentifierTypo
// ReSharper disable UseStringInterpolation

namespace ImMicro.Common.Helpers
{
    //INFO: Based on https://gist.github.com/wcharczuk/2284226
    public class DynamicRouteParser
    {
        private const char VariableStartChar = '{';
        private const char VariableEndChar = '}';
        private const char QueryStringDivider = '?';
        private const string RouteTokenPattern = @"[{0}].+?[{1}]"; //the 0 and 1 are used by the string.format function, they are the start and end characters.
        private const string VariableTokenPattern = "(?<{0}>[^,]*)"; //the <>'s denote the group name; this is used for reference for the variables later.
        private HashSet<string> Variables { get; set; }
        private string RouteFormat { get; set; }

        public DynamicRouteParser(string baseRouteTemplate)
        {
            if (string.IsNullOrWhiteSpace(baseRouteTemplate))
            {
                throw new ArgumentNullException(nameof(baseRouteTemplate));
            }

            if (baseRouteTemplate.Contains(QueryStringDivider))
            {
                baseRouteTemplate = baseRouteTemplate.Split(QueryStringDivider).First();
            }

            RouteFormat = baseRouteTemplate;
            ParseRouteFormat();
        }
 
        /// <summary>
        /// Extract variable values from a given instance of the route you're trying to parse.
        /// </summary> 
        /// <param name="route">Route for parse</param>
        /// <returns>A dictionary of Variable names mapped to values.</returns>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public List<RouteParameter> ParseRoute(string route)
        {
            var splittedList = route.Split(QueryStringDivider);

            if (splittedList.Length > 2)
            {
                throw new ArgumentException(
                    "route format is invalid, there are multiple url, querystring split char '?'.");
            }

            var baseRoute = splittedList.First();
            var queryString = splittedList.Length > 1 ? splittedList.Last() : string.Empty;

            var response = new List<RouteParameter>();
            ParseRouteParams(baseRoute, response);
            ParseQueryStringParams(queryString, response);

            return response;
        }

        #region Private Helper Methods

        private void ParseRouteFormat()
        {
            var variableList = new List<string>();
            var matchCollection = Regex.Matches
            (
                this.RouteFormat
                , string.Format(RouteTokenPattern, VariableStartChar, VariableEndChar)
                , RegexOptions.IgnoreCase
            );

            foreach (var match in matchCollection)
            {
                variableList.Add(RemoteVariableChars(match.ToString()));
            }

            Variables = new HashSet<string>(variableList);
        }
        
        private static string RemoteVariableChars(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return input;

            string result = new String(input.ToArray());
            result = result.Replace(VariableStartChar.ToString(), String.Empty)
                .Replace(VariableEndChar.ToString(), String.Empty);
            return result;
        }

        private static string WrapWithVariableChars(string input)
        {
            return string.Format("{0}{1}{2}", VariableStartChar, input, VariableEndChar);
        }

        private void ParseRouteParams(string baseRoute, List<RouteParameter> parseResponse)
        { 
            string formatUrl = new string(RouteFormat.ToArray());
            foreach (string variable in Variables)
            {
                formatUrl = formatUrl.Replace(WrapWithVariableChars(variable),
                    string.Format(VariableTokenPattern, variable));
            }

            var regex = new Regex(formatUrl, RegexOptions.IgnoreCase);
            var matchCollection = regex.Match(baseRoute);

            foreach (var variable in Variables)
            {
                var value = matchCollection.Groups[variable].Value;
                parseResponse.Add(new RouteParameter
                {
                    Key = variable,
                    Value = value,
                    Type = RouteParameterType.Route
                });
            }
        }

        private static void ParseQueryStringParams(string queryString, List<RouteParameter> parseResponse)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return;
            }

            var parsedQueryString = HttpUtility.ParseQueryString(queryString);

            foreach (string key in parsedQueryString)
            {
                parseResponse.Add(new RouteParameter
                {
                    Key = key,
                    Value = parsedQueryString[key],
                    Type = RouteParameterType.QueryString
                });
            }
        }

        #endregion

        public class RouteParameter
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public RouteParameterType Type { get; set; }
        }
    }

    public enum RouteParameterType
    {
        Route = 1,
        QueryString = 2
    }
}