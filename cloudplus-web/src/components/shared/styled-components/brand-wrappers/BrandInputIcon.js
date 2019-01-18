import styled from 'vue-styled-components';

const brandProps = { color: String };

// Create a <BrandInputIcon> Vue component that renders a <div> which
// uses the passed in color to style the radio buttonor checkbox
const BrandInputIcon = styled('div', brandProps)`  
  color: ${props => props.color};  
  font-size: 1.35rem;
  float: left;
  margin: -0.125rem 0.3125rem 0 0;
  cursor: pointer;
`;

export default BrandInputIcon;
