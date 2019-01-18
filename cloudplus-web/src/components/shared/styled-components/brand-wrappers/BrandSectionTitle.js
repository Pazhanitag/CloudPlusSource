import styled from 'vue-styled-components';

const brandProps = { color: String };

// Create a <BrandSectionTitle> Vue component that renders a <div> which uses
// the passed in color as text color, and has a bottom border
const BrandSectionTitle = styled('div', brandProps)`
  color: ${props => props.color} !important;
  border-bottom: solid 0.0625rem #dce1ea;
  font-size: 0.93rem;
  padding-bottom: 0.625rem;
  margin-bottom: 1.875rem;
  margin-top:3rem;
  font-weight:bold;
`;

export default BrandSectionTitle;
