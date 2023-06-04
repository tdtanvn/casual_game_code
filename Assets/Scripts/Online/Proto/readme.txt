Here is some command to generate code from proto
C++:
- protoc --proto_path=. --cpp_out=. CG.proto

C#:
- protoc --proto_path=. --csharp_out=. CG.proto

JS:
- pbjs -t static-module -w commonjs --keep-case -o bundle.js  ./proto/CG.proto
- pbts -o bundle.d.ts bundle.js
