import styled from 'vue-styled-components';

const brandProps = { backgroundColor: String, textColor: String };

// Create a <BrandHover> Vue component that renders a <div> which
// on hover changes the background color to the passed in color
const BrandHover = styled('div', brandProps)`
  &:hover {
    background-color: ${props => props.backgroundColor};
    color: ${props => props.textColor} !important;
  }
  & > a.router-link-active {
    background-color: ${props => props.backgroundColor};
    color: ${props => props.textColor} !important;
  }
`;

export default BrandHover;

