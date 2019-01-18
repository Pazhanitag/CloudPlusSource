import styled from 'vue-styled-components';

const componentProps = {
  color: String,
};

// Create a <BrandColorPrimary> Vue component that renders a <div> which is
// the color of the current theme
const BrandInput = styled('div', componentProps)`
  input:active, input:focus, .textarea:focus, .textarea.is-focused, .textarea:active, .textarea.is-active {
    border-color: ${props => props.color} !important;
  }
`;

export default BrandInput;
