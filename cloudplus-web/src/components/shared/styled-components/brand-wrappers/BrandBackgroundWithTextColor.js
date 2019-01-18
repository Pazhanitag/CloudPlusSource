import styled from 'vue-styled-components';

const brandProps = { backgroundColor: String, textColor: String };

// Create a <BrandBackgroundWithTextColor> Vue component that renders a <div> which
// has the passed in background and text color
const BrandBackgroundWithTextColor = styled('div', brandProps)`
  background-color: ${props => props.backgroundColor};
  color: ${props => props.textColor};
`;

export default BrandBackgroundWithTextColor;

