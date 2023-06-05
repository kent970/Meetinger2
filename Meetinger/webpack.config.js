"use strict";
var path = require("path");
var WebpackNotifierPlugin = require("webpack-notifier");
var BrowserSyncPlugin = require("browser-sync-webpack-plugin");

module.exports = {
    mode: "development", // Set the mode to "development"
    entry: ['babel-polyfill', "./wwwroot/React/src/index.js"],
    output: {
        path: path.resolve(__dirname, "./wwwroot/React/dist"),
        filename: "bundle.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                },
            },
            {
                test: /\.css$/,
                use: ["style-loader", "css-loader"],
            },
        ],
    },
    devtool: "inline-source-map",
    plugins: [new WebpackNotifierPlugin(), new BrowserSyncPlugin()],
    resolve: {
        extensions: ['.js', '.jsx'],
    }
};