# WarwickEpr.HighFieldEpr

This project is the control software for a high frequency (396.8 GHz) electron paramagnetic resonance (EPR) spectrometer
operating at the University of Warwick. The spectrometer uses a [Pico Technology 5000 series oscilloscope][PS5000] for
data acquisition and a Twickenham Scientific superconducting magnet controller to ramp the magnetic field.

The software is developed in F# using `Endorphin.Core` library (available [here][Endorphin.Core]). The 
`Endorphin.Instrument.PicoTech` library (available [here][Endorphin.Instrument.PicoTech]) is used to provide a high level
streaming API for the oscilloscope and `Endorphin.Instrument.Twickenham.MagnetController`
(avialble [here][Endorphin.Instrument.Twickenham.MagnetController]) is used to control the magnet controller.

Since communication to the magnet controller is performed via GPIB using the National Instruments VISA library, the
[NI VISA 15.5][NI-VISA] and [NI 488.2][NI-488.2] drivers are required. During both installations, ensure that you select
.NET Framework 4.5 Language Support during installation (under NI-VISA 15.5 -> Development Support).

## Maintainers

- [@anton-pt](https://twitter.com/anton-pt)
- [Jake Lishman](https://github.com/jakelishman)

[Endorphin.Core]: https://warwickepr.github.io/Endorphin.Core
[Endorphin.Instrument.PicoTech]: https://warwickepr.github.io/Endorphin.Instrument.PicoTech
[Endorphin.Instrument.Twickenham.MagnetController]: https://warwickepr.github.io/Endorphin.Instrument.Twickenham.MagnetController
[PS5000]: https://www.picotech.com/oscilloscope/5000/flexible-resolution-oscilloscope
[NI-VISA]: http://www.ni.com/download/ni-visa-15.5/5846/en/
[NI-488.2]: http://www.ni.com/download/ni-488.2-15.5/5859/en/