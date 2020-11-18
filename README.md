# DocsTextFormatter

A text formatter for condensing the documents for C64 releases. 
I used this to compile the documentation for the Legacy of the Ancients + Legend of Blacksilver EasyFlash cartridge.
I have since open-sourced the tool and added unit tests (which revealed some bugs I also fixed.)

### Processing

- Bit 7 of each byte indicates a space follows. `41 42 43 44 20 45 46 47 48` becomes `41 42 43 C4 45 46 47 48`.
- Characters outside the 20-7F range are ignored.
- Text is made uppercase.
- Multiple spaces in a row are expanded to space characters with bit 7. `20 20 20` becomes `A0 20`.
- End of stream is indicated with `00`.

Because of the simplicity of this encoding, it still compresses well with such algorithms as Exomizer, 
while minimizing the in-memory requirement of the documents themselves.
