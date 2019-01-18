import styled from 'vue-styled-components';

const brandProps = { color: String };

// Create a <BrandSpinner> Vue component that renders a <div> which is
// the color of the current theme
const BrandSpinner = styled('div', brandProps)`
  color: ${props => props.color};
  text-align: center;
  font-size: 1rem;
`;

export default BrandSpinner;

