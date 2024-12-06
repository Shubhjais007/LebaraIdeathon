import { createTheme, responsiveFontSizes } from '@mui/material/styles';
import ChiswickRegular from '../assets/fonts/chiswick-grotesque-app-regular.ttf';
import ChiswickSemibold from '../assets/fonts/chiswick-grotesque-app-semibold.ttf';
import Chiswickbold from '../assets/fonts/chiswick-grotesque-app-bold.ttf';
import GothamLight from '../assets/fonts/gotham-light.woff'

const ChiswickGrotesque700: any = {
    fontFamily: 'Chiswick Grot Lebara App',
    fontStyle: 'normal',
    fontDisplay: 'swap',
    fontWeight: 700,
    src: `
        local(Chiswick Grot Lebara App),
        local(Chiswick Grot Lebara App Bold),
        url(${Chiswickbold}) format('truetype')
    `
};

const ChiswickGrotesque600 = {
    fontFamily: 'Chiswick Grot Lebara App',
    fontStyle: 'normal',
    fontDisplay: 'swap',
    fontWeight: 600,
    src: `
        local(Chiswick Grot Lebara App),
        local(Chiswick Grot Lebara App Semi Bold),
        url(${ChiswickSemibold}) format('truetype')
    `
};

const ChiswickGrotesque400 = {
    fontFamily: 'Chiswick Grot Lebara App',
    fontStyle: 'normal',
    fontDisplay: 'swap',
    fontWeight: 400,
    src: `
        local(Chiswick Grot Lebara App),
        local(Chiswick Grot Lebara App Regular),
        url(${ChiswickRegular}) format('truetype')
    `
};

const Gotham = {
    fontFamily: 'Gotham Light Lebara App',
    fontStyle: 'normal',
    fontDisplay: 'swap',
    fontWeight: 400,
    src: `
        local(Gotham Light Lebara App),
        local(Gotham Light Lebara App Regular),
        url(${GothamLight}) format('woff')
    `
};

let theme = createTheme({
    cssVariables: true,
    palette: {
        primary: {
            main: "#0094F6"
        },
        secondary: {
            main: "#b91866"
        },
        contrastThreshold: 3,
        tonalOffset: 0.2,
    },
    typography: {
        fontFamily: [
            'Chiswick Grot Lebara App',
            'Gotham Light Lebara App',
            '-apple-system', 'BlinkMacSystemFont', "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", 'sans-serif'
        ].join(','),
        button: {
            textTransform: 'none'
        }
    },
    components: {
        MuiCssBaseline: {
            styleOverrides: {
                '@global': {
                    '@font-face': [ChiswickGrotesque700, ChiswickGrotesque600, ChiswickGrotesque400, Gotham],
                    html: {
                        WebkitFontSmoothing: 'antialiased',
                    }
                },
            },
        },
    }
})

theme = responsiveFontSizes(theme)

export default theme;