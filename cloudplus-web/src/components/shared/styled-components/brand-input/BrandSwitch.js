import styled from 'vue-styled-components';

const componentProps = {
  color: String,
};

// Create a <BrandSwitch> Vue component that renders a <label> which is
// the color of the current theme
const BrandSwitch = styled('label', componentProps)`
input:checked + .slider {
  background-color: ${props => props.color};
}

input:focus + .slider {
  box-shadow: 0 0 1px ${props => props.color};
}
`;

export default BrandSwitch;
