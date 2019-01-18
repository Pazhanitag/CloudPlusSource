import styled from 'vue-styled-components';

const componentProps = {
  color: String,
};

// Create a <BrandColorPrimary> Vue component that renders a <div> which is
// the color of the current theme
const BrandSelect = styled('div', componentProps)`
  select:focus {
    border-color: ${props => props.color} !important;
  }
`;
export default BrandSelect;
