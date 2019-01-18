import styled from 'vue-styled-components';

const brandProps = { color: String };

// Create a <BrandTextColor> Vue component that renders a <div> which uses
// the passed in color as text color
const BrandTextColor = styled('div', brandProps)`
  color: ${props => props.color} !important;
`;

export default BrandTextColor;
