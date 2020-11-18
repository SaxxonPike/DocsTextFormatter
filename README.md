# DocsTextFormatter

A text formatter for condensing the documents for C64 releases. 
I used this to compile the documentation for the Legacy of the Ancients + Legend of Blacksilver EasyFlash cartridge.
I have since open-sourced the tool and added unit tests (which revealed some bugs I also fixed.)

### Processing

- Bit 7 of each byte indicates a space follows. `41 42 43 44 20 45 46 47 48` becomes `41 42 43 C4 45 46 47 48`.
- Characters outside the `20`-`7F` range are ignored except `0D` (line break.)
- Text is made uppercase.
- Multiple spaces in a row are expanded to space characters with bit 7. `20 20 20` becomes `A0 20`.
- End of stream is indicated with `00`.

Because of the simplicity of this encoding, it still compresses well with such algorithms as Exomizer, 
while minimizing the in-memory requirement of the documents themselves.

### Future tasks

The current method does not require character maps or bit dictionaries (Huffman coding for example) and is very easy to implement
in a small amount of code. It wouldn't be that much more code to improve on the effectiveness of the encoding, however.
It would remain to be seen if further changes would gain us much in the way of space savings over extra CPU time and code space.

Since we are only using uppercase letters, only the text ranges 20-5F and 7B-7F are used. If we eliminate some characters and
make some sort of 6-bit map, we can use 2 bits to represent control characters as opposed to just 1 for spaces. We would
have 26 latin characters, 10 numbers and 28 non-alphanumerics to use. Maybe a future version of this will do the job.
