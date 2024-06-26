"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;
var _deflate2 = require("../vendor/pako/lib/zlib/deflate.js");
var _zstream = _interopRequireDefault(require("../vendor/pako/lib/zlib/zstream.js"));
function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }
function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }
function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, _toPropertyKey(descriptor.key), descriptor); } }
function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); } /*
 * noVNC: HTML5 VNC client
 * Copyright (C) 2020 The noVNC Authors
 * Licensed under MPL 2.0 (see LICENSE.txt)
 *
 * See README.md for usage and integration instructions.
 */
var Deflator = exports["default"] = /*#__PURE__*/function () {
  function Deflator() {
    _classCallCheck(this, Deflator);
    this.strm = new _zstream["default"]();
    this.chunkSize = 1024 * 10 * 10;
    this.outputBuffer = new Uint8Array(this.chunkSize);
    (0, _deflate2.deflateInit)(this.strm, _deflate2.Z_DEFAULT_COMPRESSION);
  }
  return _createClass(Deflator, [{
    key: "deflate",
    value: function deflate(inData) {
      /* eslint-disable camelcase */
      this.strm.input = inData;
      this.strm.avail_in = this.strm.input.length;
      this.strm.next_in = 0;
      this.strm.output = this.outputBuffer;
      this.strm.avail_out = this.chunkSize;
      this.strm.next_out = 0;
      /* eslint-enable camelcase */

      var lastRet = (0, _deflate2.deflate)(this.strm, _deflate2.Z_FULL_FLUSH);
      var outData = new Uint8Array(this.strm.output.buffer, 0, this.strm.next_out);
      if (lastRet < 0) {
        throw new Error("zlib deflate failed");
      }
      if (this.strm.avail_in > 0) {
        // Read chunks until done

        var chunks = [outData];
        var totalLen = outData.length;
        do {
          /* eslint-disable camelcase */
          this.strm.output = new Uint8Array(this.chunkSize);
          this.strm.next_out = 0;
          this.strm.avail_out = this.chunkSize;
          /* eslint-enable camelcase */

          lastRet = (0, _deflate2.deflate)(this.strm, _deflate2.Z_FULL_FLUSH);
          if (lastRet < 0) {
            throw new Error("zlib deflate failed");
          }
          var chunk = new Uint8Array(this.strm.output.buffer, 0, this.strm.next_out);
          totalLen += chunk.length;
          chunks.push(chunk);
        } while (this.strm.avail_in > 0);

        // Combine chunks into a single data

        var newData = new Uint8Array(totalLen);
        var offset = 0;
        for (var i = 0; i < chunks.length; i++) {
          newData.set(chunks[i], offset);
          offset += chunks[i].length;
        }
        outData = newData;
      }

      /* eslint-disable camelcase */
      this.strm.input = null;
      this.strm.avail_in = 0;
      this.strm.next_in = 0;
      /* eslint-enable camelcase */

      return outData;
    }
  }]);
}();